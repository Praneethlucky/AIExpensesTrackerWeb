using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace ExpenseTracker.Infrastructure.Persistence;

public class BillRepository : IBillRepository
{
    private readonly string _connectionString;

    public BillRepository(IOptions<DatabaseSettings> options)
    {
        _connectionString = options.Value.AzureSql;
    }

    public async Task<bool> InsertAsync(Bill bill)
    {
        const string sql = """
        INSERT INTO Bills
        (UserId, Category, Description, Amount, Frequency, StartDate, EndDate, IsActive)
        VALUES
        (@UserId, @Category, @Description, @Amount, @Frequency, @StartDate, @EndDate, 1)
        """;

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@UserId", bill.UserId);
        cmd.Parameters.AddWithValue("@Category", bill.Category);
        cmd.Parameters.AddWithValue("@Description", bill.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Amount", bill.Amount);
        cmd.Parameters.AddWithValue("@Frequency", bill.Frequency);
        cmd.Parameters.AddWithValue("@StartDate", bill.StartDate);
        cmd.Parameters.AddWithValue("@EndDate", bill.EndDate ?? (object)DBNull.Value);

        await conn.OpenAsync();
        var rows = await cmd.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<List<Bill>> GetActiveBillsAsync(int userId)
    {
        const string sql = @"
        SELECT BillId, UserId, Category, Description,
               Amount, Frequency, StartDate, EndDate,
               IsActive, CreatedAt, UpdatedAt
        FROM Bills
        WHERE UserId = @UserId
          AND IsActive = 1";

        var bills = new List<Bill>();

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var bill = new Bill(
                reader.GetInt32(1),                            // UserId
                reader.GetString(2),                           // Category
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.GetDecimal(4),                          // Amount
                reader.GetString(5),                           // Frequency
                reader.GetDateTime(6),                         // StartDate
                reader.IsDBNull(7) ? null : reader.GetDateTime(7)
            );

            // Set private fields via reflection-safe method
            typeof(Bill).GetProperty(nameof(Bill.BillId))!
                .SetValue(bill, reader.GetInt32(0));

            typeof(Bill).GetProperty(nameof(Bill.CreatedOn))!
                .SetValue(bill, reader.GetDateTime(9));

            if (!reader.IsDBNull(10))
            {
                typeof(Bill).GetProperty(nameof(Bill.UpdatedOn))!
                    .SetValue(bill, reader.GetDateTime(10));
            }

            bills.Add(bill);
        }

        return bills;
    }

    public async Task<List<Bill>> GetMonthlyBillsAsync(int userId, int year, int month)
    {
        var startOfMonth = new DateTime(year, month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        const string sql = @"
        SELECT BillId, UserId, Category, 
               Amount, Frequency,  
               IsActive, CreatedAt, UpdatedAt
        FROM Bills
        WHERE UserId = @UserId
          AND IsActive = 1
          ";

        var bills = new List<Bill>();

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
        

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var bill = new Bill(
                reader.GetInt32(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.GetDecimal(4),
                reader.GetString(5),
                reader.GetDateTime(6),
                reader.IsDBNull(7) ? null : reader.GetDateTime(7)
            );

            typeof(Bill).GetProperty(nameof(Bill.BillId))!
                .SetValue(bill, reader.GetInt32(0));

            bills.Add(bill);
        }

        return bills;
    }

    public async Task<bool> DeleteAsync(int billId, int userId)
    {
        const string sql = @"
        UPDATE Bills
        SET IsActive = 0,
            UpdatedOn = SYSUTCDATETIME()
        WHERE BillId = @BillId
          AND UserId = @UserId";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add("@BillId", SqlDbType.Int).Value = billId;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        await conn.OpenAsync();
        var rows = await cmd.ExecuteNonQueryAsync();

        return rows > 0;
    }
}