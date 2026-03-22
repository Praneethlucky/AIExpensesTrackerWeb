using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace ExpenseTracker.Infrastructure.Repositories;

public class PaymentTypeRepository : IPaymentTypeRepository
{
    private readonly ConnectionFactory _factory;

    public PaymentTypeRepository(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<PaymentType>> GetAllAsync()
    {
        var result = new List<PaymentType>();

        using SqlConnection connection = _factory.CreateConnection();

        using SqlCommand command = new SqlCommand(
            @"SELECT 
                PaymentTypeId,
                Name,
                IsActive,
                CreatedAt
              FROM PaymentTypes
              WHERE IsActive = 1",
            connection);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new PaymentType
            {
                PaymentTypeId = reader.GetInt32(0),
                Name = reader.GetString(1),
                IsActive = reader.GetBoolean(2),
                CreatedAt = reader.GetDateTime(3)
            });
        }

        return result;
    }

    public async Task<PaymentType?> GetByIdAsync(int id)
    {
        using SqlConnection connection = _factory.CreateConnection();

        using SqlCommand command = new SqlCommand(
            @"SELECT 
                PaymentTypeId,
                Name,
                IsActive,
                CreatedAt
              FROM PaymentTypes
              WHERE PaymentTypeId = @Id",
            connection);

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new PaymentType
            {
                PaymentTypeId = reader.GetInt32(0),
                Name = reader.GetString(1),
                IsActive = reader.GetBoolean(2),
                CreatedAt = reader.GetDateTime(3)
            };
        }

        return null;
    }
}