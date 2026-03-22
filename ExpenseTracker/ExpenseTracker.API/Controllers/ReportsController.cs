using ExpenseTracker.API.Controllers;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.DTOs.Reports;
using ExpenseTracker.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/reports")]
public class ReportsController : BaseController
{
    private readonly IReportService _service;

    public ReportsController(
        IReportService service)
    {
        _service = service;
    }

    [HttpGet("monthly")]
    public async Task<
        ApiResponse<MonthlyReportDto>>
        GetMonthly(
        int year,
        int month)
    {
        try
        {
            var data =
                await _service.GetMonthlyAsync(
                    UserId,
                    year,
                    month);

            return ApiResponse<MonthlyReportDto>
                .SuccessResponse(data);
        }
        catch (Exception ex)
        {
            return ApiResponse<MonthlyReportDto>
                .FailureResponse(
                    ex.Message,
                    "Report failed");
        }
    }
}