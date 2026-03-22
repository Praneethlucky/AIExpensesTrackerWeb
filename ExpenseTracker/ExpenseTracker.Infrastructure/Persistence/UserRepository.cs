using ExpenseTracker.Infrastructure.Configuration;
using ExpenseTracker.Infrastructure.Entities;
using ExpenseTracker.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace ExpenseTracker.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly ConnectionFactory _connectionFactory;


    public UserRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = """
        SELECT UserId, FullName, Email, PasswordHash, MonthlySalary, IsActive, CreatedAt
        FROM dbo.Users
        WHERE Email = @Email AND IsActive = 1
        """;

        using var conn = _connectionFactory.CreateConnection();
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

        using var conn = _connectionFactory.CreateConnection();
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

    public async Task<int> InsertAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        var query = @"
            INSERT INTO Users
            (
                Email,
                PasswordHash,
                FullName,
                MonthlySalary,
                IsActive,
                Role,
                CreatedAt
            )
            VALUES
            (
                @Email,
                @PasswordHash,
                @FullName,
                @CurrentSalary,
                @IsActive,
                @Role,
                @CreatedAt
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
        ";

        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@FullName", user.FullName);
        command.Parameters.AddWithValue("@CurrentSalary", user.CurrentSalary);
        command.Parameters.AddWithValue("@IsActive", true);
        command.Parameters.AddWithValue("@Role", "User");
        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

        await connection.OpenAsync();

        var id = (int)await command.ExecuteScalarAsync();

        return id;
    }
        

    public async Task<bool> UpdateSalaryAsync(int userId, decimal salary)
    {
        using var conn = _connectionFactory.CreateConnection();

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

    public async Task<bool> AddSalaryHistory(UserSalaryHistory history)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"INSERT INTO UserSalaryHistory
                   (UserId, Salary, EffectiveFrom, CreatedAt)
                   VALUES
                   (@UserId, @Salary, @EffectiveFrom, @CreatedAt)";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = history.UserId;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = history.Salary;
        command.Parameters.Add("@EffectiveFrom", SqlDbType.Date).Value = history.EffectiveFrom;
        command.Parameters.Add("@CreatedAt", SqlDbType.DateTime2).Value = history.CreatedAt;

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();

        return true;
    }
}
