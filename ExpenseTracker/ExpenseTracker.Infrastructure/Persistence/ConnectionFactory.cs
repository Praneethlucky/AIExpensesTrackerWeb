using ExpenseTracker.Infrastructure.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.Persistence;

public class ConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(IOptions<DatabaseSettings> options)
    {
        _connectionString = options.Value.AzureSql;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}