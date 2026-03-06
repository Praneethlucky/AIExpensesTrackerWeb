using ExpenseTracker.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.BusinessLogic.Common;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _userService.LoginAsync(request);

        if (result == null)
            return Unauthorized(
                ApiResponse<string>.FailureResponse(
                    new List<string> { "Invalid credentials" }));

        return Ok(ApiResponse<LoginResponseDto>
            .SuccessResponse(result, "Login successful"));
    }

    //[HttpPost("register")]
    //public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    //{
    //    var success = await _userService.RegisterAsync(request);

    //    if (!success)
    //        return BadRequest(ApiResponse<string>
    //            .FailureResponse(new List<string> { "Registration failed" }));

    //    return Ok(ApiResponse<string>
    //        .SuccessResponse("User created"));
    //}

    [HttpPut("update-salary")]
    public async Task<IActionResult> UpdateSalary(int userId, decimal salary)
    {
        var result = await _userService.UpdateSalaryAsync(userId, salary);

        if (!result)
            return NotFound("User not found");

        return Ok("Salary updated successfully");
    }
}