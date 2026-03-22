using Azure;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.BusinessLogic.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public async Task<IActionResult> Get()
        {

            var result = await _service.GetCategories(UserId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Retrieved All Existing Categories successfully"));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateCategoryRequest categoryRequest)
        {

            var id = await _service.CreateCategory(categoryRequest, UserId);

            return Ok(ApiResponse<object>.SuccessResponse(id, $"Created new category: {categoryRequest.Name}"));
        }
        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CategoryDto categoryRequest)
        {

            var id = await _service.UpdateCategory(categoryRequest, UserId);

            return Ok(ApiResponse<object>.SuccessResponse(id, $"Updated category: {categoryRequest.Name}"));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody]int id)
        {

            await _service.DeleteCategory(id, UserId);

            return Ok(ApiResponse<object>.SuccessResponse(id, "Deleted category"));
        }

    }
}