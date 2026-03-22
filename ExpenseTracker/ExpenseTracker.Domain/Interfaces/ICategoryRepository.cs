
namespace ExpenseTracker.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync(int userId);
        Task<Category> GetCategoriesbyIdAsync(int categoryId, int userId);

        Task<int> CreateCategoryAsync(Category category, int userId);

        Task<bool> DeleteCategoryAsync(int categoryId, int userId);
        Task<List<Category>> GetCategoriesbyNameAsync(string categoryName, int userId);
        Task<int> UpdateCategory(Category category, int userId);

    }
}