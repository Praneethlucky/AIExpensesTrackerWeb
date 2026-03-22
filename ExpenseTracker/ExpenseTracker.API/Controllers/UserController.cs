using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.BusinessLogic.Services.Implementation;
using ExpenseTracker.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {

            var response = await _userService.GetUserProfileByID(UserId);

            return Ok(ApiResponse<object>.SuccessResponse(response, "Pulled User Details successfully"));
        }
        
        [Authorize]
        [HttpPut("UpdateSalary")]
        public async Task<IActionResult> UpdateSalary(decimal salary)
        {

            var response = await _userService.UpdateSalaryAsync(UserId, salary);

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequestDTO user)
        {

            var userId = await _userService.RegisterAsync(user);

            return Ok(ApiResponse<object>.SuccessResponse(new
            {
                UserId = userId
            }, "User registered successfully"));
        }
    }
}
