using ExpenseTracker.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.BusinessLogic.Common;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly IAIService _aiService;

    public AIController(IAIService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("monthly-insight")]
    public async Task<IActionResult> GenerateMonthlyInsight(
        [FromBody] MonthlyInsightRequestDto request)
    {
        var result = await _aiService
            .GenerateMonthlyInsightAsync(request);

        return Ok(ApiResponse<MonthlyInsightResponseDto>
            .SuccessResponse(result));
    }
}