using Azure;
using ExpenseTracker.API.Controllers;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.DTOs.Bills;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.Domain.DTOs.Bills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/bills")]
public class BillsController : BaseController
{
    private readonly IBillService _service;

    public BillsController(IBillService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ApiResponse<int>> Create(
        CreateBillDto dto)
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
    public async Task<ApiResponse<List<BillDto>>> GetAll()
    {
        try
        {

            var data =
                await _service.GetAllAsync(UserId);

            return ApiResponse<List<BillDto>>
                .SuccessResponse(data);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<BillDto>>
                .FailureResponse(
                    ex.Message,
                    "Fetch failed");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateBillDto updateBill)
    {
        try
        {
            await _service.UpdateAsync(UserId, updateBill);
            return Ok(ApiResponse<object>.SuccessResponse("Updated Bill successfully"));
        }
        catch (Exception ex)
        {
            return Ok(ApiResponse<object>.FailureResponse("Update Failed"));

        }
    }


    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody]int id)
    {
        try
        {

                await _service.DeleteAsync(UserId, id);

            return Ok(ApiResponse<object>.SuccessResponse("Deleted Bill"));

        }
        catch (Exception ex)
        {
            return Ok(ApiResponse<object>.FailureResponse("Delete Failed"));

        }
    }
}