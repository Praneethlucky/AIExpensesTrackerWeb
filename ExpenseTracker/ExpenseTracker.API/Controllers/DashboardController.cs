using ExpenseTracker.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.BusinessLogic.Common;


[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("{userId}/{year}/{month}")]
    public async Task<IActionResult> GetDashboard(
        int userId,
        int year,
        int month)
    {
        var result = await _dashboardService
            .GetDashboardAsync(userId, year, month);

        return Ok(ApiResponse<DashboardSummaryDto>
            .SuccessResponse(result));
    }
}