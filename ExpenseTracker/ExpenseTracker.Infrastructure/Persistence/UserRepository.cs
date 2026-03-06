using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Configuration;

namespace ExpenseTracker.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IOptions<DatabaseSettings> options)
    {
        _connectionString = options.Value.AzureSql;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = """
        SELECT UserId, FullName, Email, PasswordHash, MonthlySalary, IsActive, CreatedAt
        FROM dbo.Users
        WHERE Email = @Email AND IsActive = 1
        """;

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Email", email);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new User(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetDecimal(4)
        );
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        const string sql = """
        SELECT UserId, FullName, Email, PasswordHash, MonthlySalary, IsActive, CreatedAt
        FROM dbo.Users
        WHERE UserId = @UserId AND IsActive = 1
        """;

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@UserId", userId);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new User(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetDecimal(4)
        );
    }

    public async Task<bool> InsertAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateSalaryAsync(int userId, decimal salary)
    {
        using var conn = new SqlConnection(_connectionString);

        var query = @"
        UPDATE Users
        SET MonthlySalary = @MonthlySalary,
            UpdatedAt = SYSUTCDATETIME()
        WHERE UserId = @UserId";

        using var command = new SqlCommand(query, conn);

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@MonthlySalary", salary);

        await conn.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }
}