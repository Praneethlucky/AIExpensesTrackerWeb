using Azure;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> Check()
    {

        return Ok(ApiResponse<object>.SuccessResponse("Checked In successfully"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var response = await _authService.Login(request);

        return Ok(ApiResponse<object>.SuccessResponse(response, "Logged In successfully"));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var response = await _authService.Refresh(request.RefreshToken);

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequestDto request)
    {
        await _authService.Logout(request.RefreshToken);

        return Ok(ApiResponse<object>.SuccessResponse(new
        {
            Success = true
        }, "User Logged Out successfully"));
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll()
    {
        var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        await _authService.LogoutAll(userId);

        return Ok(ApiResponse<object>.SuccessResponse(new
        {
            Success = true
        }, "Logged Out of All devices successfully"));
    }
}