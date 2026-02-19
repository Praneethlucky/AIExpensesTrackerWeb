using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;

public class UserService
{
    private readonly SqlConnectionFactory _connectionFactory;

    public UserService(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<(int UserId, string PasswordHash)?> GetByEmailAsync(string email)
    {
        const string sql = @"
SELECT UserId, PasswordHash
FROM Users
WHERE Email = @Email AND IsActive = 1;
";

        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Email", email);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return (
            reader.GetInt32(0),
            reader.GetString(1)
        );
    }

}