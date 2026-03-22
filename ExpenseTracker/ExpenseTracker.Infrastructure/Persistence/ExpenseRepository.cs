using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace ExpenseTracker.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ConnectionFactory _factory;

    public ExpenseRepository(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<int> InsertAsync(Expense expense)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"INSERT INTO Expenses
          (UserId,OccurrenceId,Amount,
           Description,ExpenseDate,IsActive,CreatedAt)
          OUTPUT INSERTED.ExpenseId
          VALUES
          (@UserId,@OccurrenceId,@Amount,
           @Description,@ExpenseDate,1,GETUTCDATE())",
        con);

        cmd.Parameters.AddWithValue("@UserId", expense.UserId);

        cmd.Parameters.AddWithValue(
            "@OccurrenceId",
            (object?)expense.OccurrenceId ?? DBNull.Value);

        cmd.Parameters.AddWithValue("@Amount", expense.Amount);

        cmd.Parameters.AddWithValue(
            "@Description",
            expense.Description ?? "");

        cmd.Parameters.AddWithValue(
            "@ExpenseDate",
            expense.ExpenseDate);

        await con.OpenAsync();

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task UpdateAsync(Expense expense)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"UPDATE Expenses
          SET Amount=@Amount,
              Description=@Description,
              ExpenseDate=@ExpenseDate
          WHERE ExpenseId=@Id
          AND UserId=@UserId",
        con);

        cmd.Parameters.AddWithValue("@Id", expense.ExpenseId);
        cmd.Parameters.AddWithValue("@UserId", expense.UserId);
        cmd.Parameters.AddWithValue("@Amount", expense.Amount);
        cmd.Parameters.AddWithValue("@Description", expense.Description);
        cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int expenseId, int userId)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"UPDATE Expenses
          SET IsActive = 0
          WHERE ExpenseId=@Id
          AND UserId=@UserId",
        con);

        cmd.Parameters.AddWithValue("@Id", expenseId);
        cmd.Parameters.AddWithValue("@UserId", userId);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Expense>> GetMonthlyAsync(
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
          AND IsActive=1
          AND YEAR(ExpenseDate)=@Year
          AND MONTH(ExpenseDate)=@Month",
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

    public async Task MarkOccurrencePaidAsync(int occurrenceId)
    {
        using SqlConnection con = _factory.CreateConnection();

        using SqlCommand cmd = new SqlCommand(
        @"UPDATE BillOccurrences
          SET IsPaid = 1
          WHERE OccurrenceId=@Id",
        con);

        cmd.Parameters.AddWithValue("@Id", occurrenceId);

        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }
}