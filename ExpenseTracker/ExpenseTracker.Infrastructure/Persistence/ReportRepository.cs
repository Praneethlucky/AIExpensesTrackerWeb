using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace ExpenseTracker.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ConnectionFactory _factory;

    public ReportRepository(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<BillOccurrence>> GetBillsAsync(
        int userId,
        int year,
        int month)
    {
        var list = new List<BillOccurrence>();

        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"SELECT OccurrenceId,
                 BillId,
                 DueDate,
                 Amount,
                 IsPaid
          FROM BillOccurrences
          WHERE UserId=@UserId
          AND YEAR(DueDate)=@Year
          AND MONTH(DueDate)=@Month
          AND IsActive=1",
        con);

        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Month", month);

        await con.OpenAsync();

        using SqlDataReader reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new BillOccurrence
            {
                OccurrenceId = reader.GetInt32(0),
                BillId = reader.GetInt32(1),
                DueDate = reader.GetDateTime(2),
                Amount = reader.GetDecimal(3),
                IsPaid = reader.GetBoolean(4),
                UserId = userId
            });
        }

        return list;
    }

    public async Task<List<Expense>> GetExpensesAsync(
        int userId,
        int year,
        int month)
    {
        var list = new List<Expense>();

        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"SELECT ExpenseId,
                 Amount,
                 Description,
                 ExpenseDate,
                 OccurrenceId
          FROM Expenses
          WHERE UserId=@UserId
          AND YEAR(ExpenseDate)=@Year
          AND MONTH(ExpenseDate)=@Month
          AND IsActive=1",
        con);

        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Month", month);

        await con.OpenAsync();

        using SqlDataReader reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Expense
            {
                ExpenseId = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                Description = reader.GetString(2),
                ExpenseDate = reader.GetDateTime(3),
                OccurrenceId = reader.IsDBNull(4)
                    ? null
                    : reader.GetInt32(4),
                UserId = userId
            });
        }

        return list;
    }

    public async Task<decimal> GetIncomeAsync(
        int userId,
        int year,
        int month)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"SELECT ISNULL(SUM(Amount),0)
          FROM Income
          WHERE UserId=@UserId
          AND YEAR(IncomeDate)=@Year
          AND MONTH(IncomeDate)=@Month",
        con);

        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Month", month);

        await con.OpenAsync();

        return (decimal)await cmd.ExecuteScalarAsync();
    }
}