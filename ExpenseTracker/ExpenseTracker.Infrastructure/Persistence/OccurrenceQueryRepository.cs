using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

public class OccurrenceQueryRepository
    : IOccurrenceQueryRepository
{
    private readonly ConnectionFactory _factory;

    public OccurrenceQueryRepository(
        ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<BillOccurrence>>
        GetOccurrencesAsync(
        int userId,
        int year,
        int month)
    {
        var list = new List<BillOccurrence>();

        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"SELECT OccurrenceId,
                 bo.BillId,
                 DueDate,
                 bo.Amount,
                 bo.IsPaid,
                 c.Name as CategoryName,
                 c.Color as Color,
                 c.Icon as Icon,
                 pt.Name as PaymentType
        from BillOccurrences bo inner join Bills b on bo.BillId = b.BillId
          inner join Categories c on b.CategoryId = c.CategoryId
          inner join PaymentTypes pt on b.PaymentTypeId = pt.PaymentTypeId
          WHERE bo.UserId=@u
          AND YEAR(DueDate)=@y
          AND MONTH(DueDate)=@m
          AND bo.IsActive=1",
        con);

        cmd.Parameters.AddWithValue("@u", userId);
        cmd.Parameters.AddWithValue("@y", year);
        cmd.Parameters.AddWithValue("@m", month);

        await con.OpenAsync();

        using var r = await cmd.ExecuteReaderAsync();

        while (await r.ReadAsync())
        {
            list.Add(new BillOccurrence
            {
                OccurrenceId = r.GetInt32(0),
                BillId = r.GetInt32(1),
                DueDate = r.GetDateTime(2),
                Amount = r.GetDecimal(3),
                IsPaid = r.GetBoolean(4),
                CategoryName = r.GetString(5),
                CategoryColor = r.GetString(6),
                CategoryIcon = r.GetString(7),
                PaymentTypeName = r.GetString(8),
                UserId = userId
            });
        }

        return list;
    }

    public async Task<string> GetBillNameAsync(int billId)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"SELECT Name
          FROM Bills
          WHERE BillId=@id",
        con);

        cmd.Parameters.AddWithValue("@id", billId);

        await con.OpenAsync();

        return (string)await cmd.ExecuteScalarAsync();
    }

    public async Task<decimal> GetExpensesTotalAsync(
        int userId,
        int year,
        int month)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"SELECT ISNULL(SUM(Amount),0)
          FROM Expenses
          WHERE UserId=@u
          AND YEAR(ExpenseDate)=@y
          AND MONTH(ExpenseDate)=@m",
        con);

        cmd.Parameters.AddWithValue("@u", userId);
        cmd.Parameters.AddWithValue("@y", year);
        cmd.Parameters.AddWithValue("@m", month);

        await con.OpenAsync();

        return (decimal)await cmd.ExecuteScalarAsync();
    }

    public async Task<decimal> GetIncomeTotalAsync(
    int userId,
    int year,
    int month)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"SELECT ISNULL(SUM(Amount),0)
      FROM Income
      WHERE UserId=@u
      AND YEAR(IncomeDate)=@y
      AND MONTH(IncomeDate)=@m",
        con);

        cmd.Parameters.AddWithValue("@u", userId);
        cmd.Parameters.AddWithValue("@y", year);
        cmd.Parameters.AddWithValue("@m", month);

        await con.OpenAsync();

        return (decimal)await cmd.ExecuteScalarAsync();
    }

    public async Task<decimal> GetSalaryAsync(
    int userId,
    int year,
    int month)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"SELECT TOP 1 Salary
      FROM UserSalaryHistory
      WHERE UserId=@u
      AND MONTH(EffectiveFrom) = MONTH(@date)
      ORDER BY EffectiveFrom DESC",
        con);

        cmd.Parameters.AddWithValue("@u", userId);

        cmd.Parameters.AddWithValue(
            "@date",
            new DateTime(year, month, 1));

        await con.OpenAsync();

        var result = await cmd.ExecuteScalarAsync();

        if (result == null || result == DBNull.Value)
            return 0;

        return (decimal)result;
    }
}