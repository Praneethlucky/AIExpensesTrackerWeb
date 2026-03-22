using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Domain.Interfaces.Services;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.Domain.DTOs.PaymentTypes;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/paymenttypes")]
public class PaymentTypesController : ControllerBase
{
    private readonly IPaymentTypeService _service;

    public PaymentTypesController(IPaymentTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ApiResponse<List<PaymentTypeDto>>> GetAll()
    {
        try
        {
            var data = await _service.GetAll();

            return ApiResponse<List<PaymentTypeDto>>
                .SuccessResponse(data, message: "Retrieved all Payment Types");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<PaymentTypeDto>>
                .FailureResponse(
                    ex.Message,
                    "Failed to fetch payment types");
        }
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<PaymentTypeDto>> Get(int id)
    {
        try
        {
            var data = await _service.GetById(id);

            return ApiResponse<PaymentTypeDto>
                .SuccessResponse(data, message: $"Retrieved Payment Type for Id: {id}");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaymentTypeDto>
                .FailureResponse(
                    ex.Message,
                    "Failed to fetch payment type");
        }
    }
}