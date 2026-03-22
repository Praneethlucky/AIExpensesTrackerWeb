using AutoMapper;
using ExpenseTracker.BusinessLogic.DTO;
using ExpenseTracker.BusinessLogic.Exceptions;
using ExpenseTracker.BusinessLogic.Services.Interfaces;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Entities;

namespace ExpenseTracker.BusinessLogic.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;


        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<List<Category>> GetCategoriesbyNameAsync(string categoryName, int userId)
        {
            return _repo.GetCategoriesbyNameAsync(categoryName, userId);
        }
        
        public Task<Category> GetCategoriesbyIdAsync(int categoryId, int userId)
        {
            return _repo.GetCategoriesbyIdAsync(categoryId, userId);
        }
        public async Task<List<CategoryDto>> GetCategories(int userId)
        {
            var categories = await _repo.GetCategoriesAsync(userId);
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<int> CreateCategory(CreateCategoryRequest category, int userId)
        {
            var existingCategory = await _repo.GetCategoriesbyNameAsync(category.Name, userId);
            if (existingCategory.Count>0)
            {
                throw new CategoryAlreadyExists(category.Name);
            }
            return await _repo.CreateCategoryAsync(_mapper.Map<Category>(category), userId);
        }

        public async Task<bool> DeleteCategory(int categoryId, int userId)
        {
            var category = await GetCategoriesbyIdAsync(categoryId, userId);
            if(category == null)
            {
                throw new CategoryNotFoundException();
            }
            
            if(category.IsSystem == true)
            {
                throw new SystemCategoryException(category.Name);
            }
            return await _repo.DeleteCategoryAsync(categoryId, userId);
        }
        public async Task<int> UpdateCategory(CategoryDto category, int userId)
        {
            if (category.IsSystem)
            {
                throw new SystemCategoryException(category.Name);
            }
            await _repo.UpdateCategory(_mapper.Map<Category>(category), userId);
            return category.CategoryId;
        }

    }
}