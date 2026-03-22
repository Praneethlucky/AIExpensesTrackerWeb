using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Infrastructure.Persistence
{
    public class DebtRepository : IDebtRepository
    {
        private readonly ConnectionFactory _factory;

        public DebtRepository(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> CreateDebt(Debt debt)
        {
            using var con = _factory.CreateConnection();

            var cmd = con.CreateCommand();

            cmd.CommandText = @"
        INSERT INTO Debts
        (UserId,PersonName,Type,TotalAmount,Description)
        VALUES
        (@UserId,@PersonName,@Type,@Amount,@Description);

        SELECT SCOPE_IDENTITY();
        ";

            cmd.Parameters.AddWithValue("@UserId", debt.UserId);
            cmd.Parameters.AddWithValue("@PersonName", debt.PersonName);
            cmd.Parameters.AddWithValue("@Type", debt.Type);
            cmd.Parameters.AddWithValue("@Amount", debt.TotalAmount);
            cmd.Parameters.AddWithValue("@Description", debt.Description ?? "");

            await con.OpenAsync();

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }


        public async Task AddPayment(DebtPayment p)
        {
            using var con = _factory.CreateConnection();

            var cmd = con.CreateCommand();

            cmd.CommandText = @"
        INSERT INTO DebtPayments
        (DebtId,UserId,Amount,PaymentDate)
        VALUES
        (@DebtId,@UserId,@Amount,@Date)
        ";

            cmd.Parameters.AddWithValue("@DebtId", p.DebtId);
            cmd.Parameters.AddWithValue("@UserId", p.UserId);
            cmd.Parameters.AddWithValue("@Amount", p.Amount);
            cmd.Parameters.AddWithValue("@Date", p.PaymentDate);

            await con.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<List<Debt>> GetDebts(int userId)
        {
            var list = new List<Debt>();

            using var con = _factory.CreateConnection();

            var cmd = con.CreateCommand();

            cmd.CommandText = @"
        SELECT
    d.DebtId,
    d.PersonName,
    d.Type,
    d.TotalAmount,
    ISNULL(SUM(p.Amount),0) Paid,
    d.TotalAmount - ISNULL(SUM(p.Amount),0) Pending
FROM Debts d
LEFT JOIN DebtPayments p
    ON d.DebtId = p.DebtId
WHERE d.UserId = @UserId
GROUP BY
    d.DebtId,
    d.PersonName,
    d.Type,
    d.TotalAmount
";

            cmd.Parameters.AddWithValue("@UserId", userId);

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Debt
                {
                    DebtId = reader.GetInt32(0),
                    PersonName = reader.GetString(1),
                    Type = reader.GetString(2),
                    TotalAmount = reader.GetDecimal(3),
                    Paid = reader.GetDecimal(4),
                    Pending = reader.GetDecimal(5)
                });
            }

            return list;
        }


        public async Task<DebtSummary> GetSummary(int userId)
        {
            using var con = _factory.CreateConnection();

            var cmd = con.CreateCommand();

            cmd.CommandText = @"

SELECT
SUM(CASE WHEN Type='GIVEN' THEN Pending ELSE 0 END),
SUM(CASE WHEN Type='TAKEN' THEN Pending ELSE 0 END)
FROM
(
    SELECT
        d.Type,
        d.TotalAmount - ISNULL(SUM(p.Amount),0) Pending
    FROM Debts d
    LEFT JOIN DebtPayments p
        ON d.DebtId = p.DebtId
    WHERE d.UserId = @UserId
    GROUP BY d.DebtId,d.Type,d.TotalAmount
)x
";

            cmd.Parameters.AddWithValue("@UserId", userId);

            await con.OpenAsync();

            using var r = await cmd.ExecuteReaderAsync();

            var result = new DebtSummary();

            if (await r.ReadAsync())
            {
                result.GivenPending = r.IsDBNull(0) ? 0 : r.GetDecimal(0);
                result.TakenPending = r.IsDBNull(1) ? 0 : r.GetDecimal(1);
            }

            return result;
        }
    }
}
