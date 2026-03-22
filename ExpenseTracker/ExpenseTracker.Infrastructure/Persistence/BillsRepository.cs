using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

public class BillRepository : IBillRepository
{
    private readonly ConnectionFactory _factory;

    public BillRepository(
        ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<bool> ExistsAsync(
        int userId,
        string name)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"SELECT COUNT(1)
          FROM Bills
          WHERE UserId=@u
          AND Name=@n
          AND IsActive=1",
        con);

        cmd.Parameters.AddWithValue("@u", userId);
        cmd.Parameters.AddWithValue("@n", name);

        await con.OpenAsync();

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<int> InsertBillAsync(
        Bill bill)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"INSERT INTO Bills
          (UserId,Name,Amount, Frequency,
           CategoryId,PaymentTypeId,
           IsActive,CreatedAt)
          OUTPUT INSERTED.BillId
          VALUES
          (@u,@n,@a, @f,@c,@p,1,GETUTCDATE())",
        con);

        cmd.Parameters.AddWithValue("@u", bill.UserId);
        cmd.Parameters.AddWithValue("@n", bill.Name);
        cmd.Parameters.AddWithValue("@a", bill.Amount);
        cmd.Parameters.AddWithValue("@f", bill.Frequency);
        cmd.Parameters.AddWithValue("@c", bill.CategoryId);
        cmd.Parameters.AddWithValue("@p", bill.PaymentTypeId);

        await con.OpenAsync();

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task InsertRuleAsync(
        RecurringRule rule)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"INSERT INTO RecurringRules
          (BillId,Frequency,StartDate,
           DayOfMonth,DayOfWeek,
           MonthOfYear,EndDate,NextRunDate,IsActive)
          VALUES
          (@b,@f,@s,@dom,@dow,@moy,@e,@nrd,1)",
        con);

        cmd.Parameters.AddWithValue("@b", rule.BillId);
        cmd.Parameters.AddWithValue("@f", rule.Frequency);
        cmd.Parameters.AddWithValue("@s", rule.StartDate);

        cmd.Parameters.AddWithValue(
            "@dom",
            (object?)rule.DayOfMonth
            ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@dow",
            (object?)rule.DayOfWeek
            ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@moy",
            (object?)rule.MonthOfYear
            ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@e",
            (object?)rule.EndDate
            ?? DBNull.Value);
        cmd.Parameters.AddWithValue(
            "@nrd",
            (object?)rule.NextRunDate
            ?? DBNull.Value);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Bill>> GetAllAsync(
        int userId)
    {
        var list = new List<Bill>();

        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"SELECT BillId,Name,Amount,
                 CategoryId,PaymentTypeId, Frequency
          FROM Bills
          WHERE UserId=@u
          AND IsActive=1",
        con);

        cmd.Parameters.AddWithValue("@u", userId);

        await con.OpenAsync();

        using var reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Bill
            {
                BillId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Amount = reader.GetDecimal(2),
                CategoryId = reader.GetInt32(3),
                PaymentTypeId = reader.GetInt32(4),
                Frequency = reader.GetString(5),
                UserId = userId
            });
        }

        return list;
    }

    public async Task UpdateAsync(
        Bill bill)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"UPDATE Bills
          SET Name=@n,
              Amount=@a,
              CategoryId=@c,
              PaymentTypeId=@p
          WHERE BillId=@id
          AND UserId=@u",
        con);

        cmd.Parameters.AddWithValue("@id", bill.BillId);
        cmd.Parameters.AddWithValue("@u", bill.UserId);
        cmd.Parameters.AddWithValue("@n", bill.Name);
        cmd.Parameters.AddWithValue("@a", bill.Amount);
        cmd.Parameters.AddWithValue("@c", bill.CategoryId);
        cmd.Parameters.AddWithValue("@p", bill.PaymentTypeId);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(
        int billId,
        int userId)
    {
        using var con = _factory.CreateConnection();

        using var cmd = new SqlCommand(
        @"UPDATE Bills
          SET IsActive=0
          WHERE BillId=@id
          AND UserId=@u",
        con);

        cmd.Parameters.AddWithValue("@id", billId);
        cmd.Parameters.AddWithValue("@u", userId);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }
}