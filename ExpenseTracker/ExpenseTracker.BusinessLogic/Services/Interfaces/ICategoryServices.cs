
using ExpenseTracker.BusinessLogic.DTO;

namespace ExpenseTracker.BusinessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesbyNameAsync(string categoryName, int userId);
        Task<Category> GetCategoriesbyIdAsync(int categoryId, int userId);
        Task<List<CategoryDto>> GetCategories(int userId);

        Task<int> CreateCategory(CreateCategoryRequest category, int userId);
        Task<int> UpdateCategory(CategoryDto category, int userId);

        Task<bool> DeleteCategory(int categoryId, int userId);
    }
}