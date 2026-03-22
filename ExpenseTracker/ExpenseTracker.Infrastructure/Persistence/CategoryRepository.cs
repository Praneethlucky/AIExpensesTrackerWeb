using ExpenseTracker.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExpenseTracker.Infrastructure.Persistence
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public CategoryRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Category>> GetCategoriesAsync(int userId)
        {
            var categories = new List<Category>();

            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    CategoryId,
                    UserId,
                    Name,
                    Type,
                    Icon,
                    Color,
                    IsSystem,
                    CreatedAt
                FROM Categories
                WHERE IsSystem = 1
                   OR UserId = @UserId
                ORDER BY Name
            ";

            var param = command.CreateParameter();
            param.ParameterName = "@UserId";
            param.Value = userId;

            command.Parameters.Add(param);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    CategoryId = Convert.ToInt32(reader["CategoryId"]),
                    UserId = reader["UserId"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["UserId"]),

                    Name = reader["Name"].ToString(),
                    Type = reader["Type"].ToString(),
                    Icon = reader["Icon"].ToString(),
                    Color = reader["Color"].ToString(),
                    IsSystem = Convert.ToBoolean(reader["IsSystem"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                });
            }

            return categories;
        }
        
        public async Task<List<Category>> GetCategoriesbyNameAsync(string categoryName, int userId)
        {
            var categories = new List<Category>();

            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    CategoryId,
                    UserId,
                    Name,
                    Type,
                    Icon,
                    Color,
                    IsSystem,
                    CreatedAt
                FROM Categories
                WHERE UPPER(Name) = UPPER(@Name)
                  AND UserId = @UserId
                ORDER BY Name
            ";

            var param = command.CreateParameter();
            param.ParameterName = "@Name";
            param.Value = categoryName;

            var param1 = command.CreateParameter();
            param1.ParameterName = "@UserId";
            param1.Value = userId;

            command.Parameters.Add(param);
            command.Parameters.Add(param1);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    CategoryId = Convert.ToInt32(reader["CategoryId"]),
                    UserId = reader["UserId"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["UserId"]),

                    Name = reader["Name"].ToString(),
                    Type = reader["Type"].ToString(),
                    Icon = reader["Icon"].ToString(),
                    Color = reader["Color"].ToString(),
                    IsSystem = Convert.ToBoolean(reader["IsSystem"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                });
            }

            return categories;
        }
        

        public async Task<Category> GetCategoriesbyIdAsync(int categoryId, int userId)
        {
            var category = new Category();

            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    CategoryId,
                    UserId,
                    Name,
                    Type,
                    Icon,
                    Color,
                    IsSystem,
                    CreatedAt
                FROM Categories
                WHERE CategoryId = @CategoryId
                  AND UserId = @UserId
                ORDER BY Name
            ";

            var param = command.CreateParameter();
            param.ParameterName = "@categoryId";
            param.Value = categoryId;

            var param1 = command.CreateParameter();
            param1.ParameterName = "@UserId";
            param1.Value = userId;

            command.Parameters.Add(param);
            command.Parameters.Add(param1);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                category = new Category
                {
                    CategoryId = Convert.ToInt32(reader["CategoryId"]),
                    UserId = reader["UserId"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["UserId"]),

                    Name = reader["Name"].ToString(),
                    Type = reader["Type"].ToString(),
                    Icon = reader["Icon"].ToString(),
                    Color = reader["Color"].ToString(),
                    IsSystem = Convert.ToBoolean(reader["IsSystem"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                };
            }

            return category;
        }

        public async Task<int> CreateCategoryAsync(Category category, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Categories
                (
                    UserId,
                    Name,
                    Type,
                    Icon,
                    Color,
                    IsSystem,
                    CreatedAt
                )
                VALUES
                (
                    @UserId,
                    @Name,
                    @Type,
                    @Icon,
                    @Color,
                    0,
                    GETDATE()
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            AddParam(command, "@UserId", userId);
            AddParam(command, "@Name", category.Name);
            AddParam(command, "@Type", category.Type);
            AddParam(command, "@Icon", category.Icon);
            AddParam(command, "@Color", category.Color);
            await connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM Categories
                WHERE CategoryId = @CategoryId
                  AND UserId = @UserId
                  AND IsSystem = 0
            ";

            AddParam(command, "@CategoryId", categoryId);
            AddParam(command, "@UserId", userId);
            await connection.OpenAsync();

            var rows = await command.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<int> UpdateCategory(Category category, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                UPDATE Categories
                SET Name = @Name, Icon = @Icon
                WHERE CategoryId = @CategoryId
                  AND UserId = @UserId
                  AND IsSystem = 0
            ";

            AddParam(command, "@CategoryId", category.CategoryId);
            AddParam(command, "@Name", category.Name);
            AddParam(command, "@Icon", category.Icon);
            AddParam(command, "@UserId", userId);
            await connection.OpenAsync();

            var rows = await command.ExecuteNonQueryAsync();

            return category.CategoryId;
        }


        private void AddParam(IDbCommand command, string name, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;

            command.Parameters.Add(param);
        }
    }
}