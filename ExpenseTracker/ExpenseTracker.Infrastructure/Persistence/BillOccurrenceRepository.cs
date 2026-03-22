using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace ExpenseTracker.Infrastructure.Repositories;

public class BillOccurrenceRepository
    : IBillOccurrenceRepository
{
    private readonly ConnectionFactory _factory;

    public BillOccurrenceRepository(
        ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<bool> ExistsAsync(
        int billId,
        DateTime dueDate)
    {
        using SqlConnection con =
            _factory.CreateConnection();

        using SqlCommand cmd =
            new SqlCommand(
        @"SELECT COUNT(1)
          FROM BillOccurrences
          WHERE BillId=@BillId
          AND DueDate=@DueDate
          AND IsActive=1",
        con);

        cmd.Parameters.AddWithValue(
            "@BillId",
            billId);

        cmd.Parameters.AddWithValue(
            "@DueDate",
            dueDate);

        await con.OpenAsync();

        int count =
            (int)await cmd.ExecuteScalarAsync();

        return count > 0;
    }

    public async Task InsertAsync(
        BillOccurrence o)
    {
        using SqlConnection con =
            _factory.CreateConnection();

        using SqlCommand cmd =
            new SqlCommand(
        @"INSERT INTO BillOccurrences
          (BillId,UserId,DueDate,
           Amount,IsPaid,IsActive,CreatedAt)
          VALUES
          (@BillId,@UserId,@DueDate,
           @Amount,0,1,GETUTCDATE())",
        con);

        cmd.Parameters.AddWithValue(
            "@BillId",
            o.BillId);

        cmd.Parameters.AddWithValue(
            "@UserId",
            o.UserId);

        cmd.Parameters.AddWithValue(
            "@DueDate",
            o.DueDate);

        cmd.Parameters.AddWithValue(
            "@Amount",
            o.Amount);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<BillOccurrence>>
        GetMonthlyAsync(
        int userId,
        int year,
        int month)
    {
        var list =
            new List<BillOccurrence>();

        using SqlConnection con =
            _factory.CreateConnection();

        using SqlCommand cmd =
            new SqlCommand(
        @"SELECT OccurrenceId,
                 BillId,
                 DueDate,
                 Amount,
                 IsPaid
          FROM BillOccurrences
          WHERE UserId=@UserId
          AND IsActive=1
          AND YEAR(DueDate)=@Year
          AND MONTH(DueDate)=@Month",
        con);

        cmd.Parameters.AddWithValue(
            "@UserId",
            userId);

        cmd.Parameters.AddWithValue(
            "@Year",
            year);

        cmd.Parameters.AddWithValue(
            "@Month",
            month);

        await con.OpenAsync();

        using SqlDataReader reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(
                new BillOccurrence
                {
                    OccurrenceId =
                        reader.GetInt32(0),

                    BillId =
                        reader.GetInt32(1),

                    DueDate =
                        reader.GetDateTime(2),

                    Amount =
                        reader.GetDecimal(3),

                    IsPaid =
                        reader.GetBoolean(4),

                    UserId = userId
                });
        }

        return list;
    }

    public async Task MarkPaidAsync(
        int occurrenceId, int userId)
    {
        using SqlConnection con =
            _factory.CreateConnection();

        using SqlCommand cmd =
            new SqlCommand(
        @"UPDATE BillOccurrences
          SET IsPaid = 1
          WHERE OccurrenceId=@Id and UserId = @UserId and IsActive = 1",
        con);

        cmd.Parameters.AddWithValue(
            "@Id",
            occurrenceId);
        
        cmd.Parameters.AddWithValue(
            "@UserId",
            userId);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<BillOccurrence>>
        GetUnpaidAsync(
        int userId)
    {
        var list =
            new List<BillOccurrence>();

        using SqlConnection con =
            _factory.CreateConnection();

        using SqlCommand cmd =
            new SqlCommand(
        @"SELECT OccurrenceId,
                 BillId,
                 DueDate,
                 Amount,
                 IsPaid
          FROM BillOccurrences
          WHERE UserId=@UserId
          AND IsPaid=0
          AND IsActive=1",
        con);

        cmd.Parameters.AddWithValue(
            "@UserId",
            userId);

        await con.OpenAsync();

        using SqlDataReader reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(
                new BillOccurrence
                {
                    OccurrenceId =
                        reader.GetInt32(0),

                    BillId =
                        reader.GetInt32(1),

                    DueDate =
                        reader.GetDateTime(2),

                    Amount =
                        reader.GetDecimal(3),

                    IsPaid =
                        reader.GetBoolean(4),

                    UserId = userId
                });
        }

        return list;
    }
}