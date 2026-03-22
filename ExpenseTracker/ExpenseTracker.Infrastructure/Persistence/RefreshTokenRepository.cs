using ExpenseTracker.Infrastructure.Entities;
using ExpenseTracker.Infrastructure.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExpenseTracker.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public RefreshTokenRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SaveAsync(RefreshToken token)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"INSERT INTO RefreshTokens
                    (Id,UserId,Token,DeviceId,CreatedAt,ExpiresAt,Revoked)
                    VALUES
                    (@Id,@UserId,@Token,@DeviceId,@CreatedAt,@ExpiresAt,@Revoked)";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = token.Id;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = token.UserId;
        command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token.Token;
        command.Parameters.Add("@DeviceId", SqlDbType.NVarChar).Value = token.DeviceId;
        command.Parameters.Add("@CreatedAt", SqlDbType.DateTime2).Value = token.CreatedAt;
        command.Parameters.Add("@ExpiresAt", SqlDbType.DateTime2).Value = token.ExpiresAt;
        command.Parameters.Add("@Revoked", SqlDbType.Bit).Value = token.Revoked;

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT * FROM RefreshTokens WHERE Token=@Token";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        if (!reader.Read())
            return null;

        return new RefreshToken
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
            Token = reader.GetString(reader.GetOrdinal("Token")),
            DeviceId = reader.GetString(reader.GetOrdinal("DeviceId")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            ExpiresAt = reader.GetDateTime(reader.GetOrdinal("ExpiresAt")),
            Revoked = reader.GetBoolean(reader.GetOrdinal("Revoked"))
        };
    }

    public async Task RevokeAsync(string token)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "UPDATE RefreshTokens SET Revoked=1 WHERE Token=@Token";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token;

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task RevokeAllForUser(Int32 userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"UPDATE RefreshTokens 
                    SET Revoked=1
                    WHERE UserId=@UserId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}