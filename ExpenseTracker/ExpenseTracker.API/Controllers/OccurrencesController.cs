using ExpenseTracker.API.Controllers;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.DTOs.Occurrences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Authorize]
[ApiController]
[Route("api/occurrences")]
public class OccurrencesController
    : BaseController
{
    private readonly IOccurrenceQueryService _service;

    public OccurrencesController(
        IOccurrenceQueryService service)
    {
        _service = service;
    }

    [HttpGet("monthly")]
    public async Task<
        ApiResponse<MonthlyOccurrencesDto>>
        GetMonthly(
        int year,
        int month)
    {

        var data =
            await _service.GetMonthlyAsync(
                UserId,
                year,
                month);

        return ApiResponse<
            MonthlyOccurrencesDto>
            .SuccessResponse(data);
    }

    [HttpPut("MarkPaid")]
    public async Task<IActionResult>
        MarkPaid(
        [FromBody]int occurrenceId)
    {
            await _service.MarkPaidAsync(occurrenceId, UserId);

        return Ok(ApiResponse<object>.SuccessResponse("Paid Successfully"));

    }
}