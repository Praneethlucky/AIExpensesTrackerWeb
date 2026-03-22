using ExpenseTracker.API.Controllers;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.Domain.DTOs.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/expenses")]
public class ExpensesController : BaseController
{
    private readonly IExpenseService _service;

    public ExpensesController(
        IExpenseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ApiResponse<int>> Create(
        CreateExpenseDto dto)
    {
        try
        {

            var id = await _service.CreateAsync(
                UserId,
                dto);

            return ApiResponse<int>
                .SuccessResponse(id);
        }
        catch (Exception ex)
        {
            return ApiResponse<int>
                .FailureResponse(
                    ex.Message,
                    "Create failed");
        }
    }

    [HttpGet]
    public async Task<ApiResponse<List<ExpenseDto>>> GetMonthly(
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

            return ApiResponse<List<ExpenseDto>>
                .SuccessResponse(data);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<ExpenseDto>>
                .FailureResponse(
                    ex.Message,
                    "Fetch failed");
        }
    }
}