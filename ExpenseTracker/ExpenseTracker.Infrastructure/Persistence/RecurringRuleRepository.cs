using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace ExpenseTracker.Infrastructure.Repositories;

public class RecurringRuleRepository : IRecurringRuleRepository
{
    private readonly ConnectionFactory _factory;

    public RecurringRuleRepository(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task InsertAsync(RecurringRule rule)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"INSERT INTO RecurringRules
          (BillId,Frequency,StartDate,
           DayOfMonth,DayOfWeek,MonthOfYear,
           EndDate,IsActive)
          VALUES
          (@BillId,@Frequency,@StartDate,
           @DayOfMonth,@DayOfWeek,@MonthOfYear,
           @EndDate,1)",
        con);

        cmd.Parameters.AddWithValue("@BillId", rule.BillId);
        cmd.Parameters.AddWithValue("@Frequency", rule.Frequency);
        cmd.Parameters.AddWithValue("@StartDate", rule.StartDate);

        cmd.Parameters.AddWithValue(
            "@DayOfMonth",
            (object?)rule.DayOfMonth ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@DayOfWeek",
            (object?)rule.DayOfWeek ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@MonthOfYear",
            (object?)rule.MonthOfYear ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@EndDate",
            (object?)rule.EndDate ?? DBNull.Value);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(RecurringRule rule)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"UPDATE RecurringRules
          SET Frequency=@Frequency,
              StartDate=@StartDate,
              DayOfMonth=@DayOfMonth,
              DayOfWeek=@DayOfWeek,
              MonthOfYear=@MonthOfYear,
              EndDate=@EndDate
          WHERE BillId=@BillId",
        con);

        cmd.Parameters.AddWithValue("@BillId", rule.BillId);
        cmd.Parameters.AddWithValue("@Frequency", rule.Frequency);
        cmd.Parameters.AddWithValue("@StartDate", rule.StartDate);

        cmd.Parameters.AddWithValue(
            "@DayOfMonth",
            (object?)rule.DayOfMonth ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@DayOfWeek",
            (object?)rule.DayOfWeek ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@MonthOfYear",
            (object?)rule.MonthOfYear ?? DBNull.Value);

        cmd.Parameters.AddWithValue(
            "@EndDate",
            (object?)rule.EndDate ?? DBNull.Value);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int billId)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"UPDATE RecurringRules
          SET IsActive = 0
          WHERE BillId=@BillId",
        con);

        cmd.Parameters.AddWithValue("@BillId", billId);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<RecurringRule?> GetByBillIdAsync(int billId)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"SELECT
            RuleId,
            BillId,
            Frequency,
            StartDate,
            DayOfMonth,
            DayOfWeek,
            MonthOfYear,
            NextRunDate,
            EndDate,
            IsActive
          FROM RecurringRules
          WHERE BillId=@BillId
          AND IsActive=1",
        con);

        cmd.Parameters.AddWithValue("@BillId", billId);

        await con.OpenAsync();

        using SqlDataReader reader =
            await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new RecurringRule
            {
                RuleId = reader.GetInt32(0),
                BillId = reader.GetInt32(1),
                Frequency = reader.GetString(2),
                StartDate = reader.GetDateTime(3),
                DayOfMonth = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                DayOfWeek = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                MonthOfYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                NextRunDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                EndDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                IsActive = reader.GetBoolean(8)
            };
        }

        return null;
    }

    public async Task<List<RecurringRule>> GetActiveRulesAsync(int userId)
    {
        var list = new List<RecurringRule>();

        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"SELECT r.RuleId,
                 r.BillId,
                 r.Frequency,
                 r.StartDate,
                 r.DayOfMonth,
                 r.DayOfWeek,
                 r.MonthOfYear,
                 r.NextRunDate,
                 r.EndDate,
                 r.IsActive
          FROM RecurringRules r
          JOIN Bills b ON b.BillId = r.BillId
          WHERE b.UserId=@UserId
          AND r.IsActive=1
          AND b.IsActive=1",
        con);

        cmd.Parameters.AddWithValue("@UserId", userId);

        await con.OpenAsync();

        using SqlDataReader reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new RecurringRule
            {
                RuleId = reader.GetInt32(0),
                BillId = reader.GetInt32(1),
                Frequency = reader.GetString(2),
                StartDate = reader.GetDateTime(3),
                DayOfMonth = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                DayOfWeek = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                MonthOfYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                NextRunDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                EndDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                IsActive = reader.GetBoolean(9)
            });
        }

        return list;
    }

    public async Task UpdateNextRunAsync(
    int ruleId,
    DateTime nextRunDate)
    {
        using var con =
            _factory.CreateConnection();

        using var cmd =
            new SqlCommand(
                @"UPDATE RecurringRules
              SET NextRunDate = @next
              WHERE RuleId = @id",
                con);

        cmd.Parameters.AddWithValue(
            "@id", ruleId);

        cmd.Parameters.AddWithValue(
            "@next", nextRunDate);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }
}