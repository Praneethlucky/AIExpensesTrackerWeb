using ExpenseTracker.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.BusinessLogic.Common;


[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly IBillService _billService;

    public BillsController(IBillService billService)
    {
        _billService = billService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBill(
        [FromBody] BillCreateDto dto)
    {
        var result = await _billService.CreateBillAsync(dto);

        return Ok(ApiResponse<BillResponseDto>
            .SuccessResponse(result, "Bill created"));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBills(int userId)
    {
        var bills = await _billService.GetUserBillsAsync(userId);

        return Ok(ApiResponse<List<BillCreateDto>>
            .SuccessResponse(bills));
    }

    [HttpDelete("{billId}/{userId}")]
    public async Task<IActionResult> DeleteBill(
        int billId,
        int userId)
    {
        var success = await _billService.DeleteBillAsync(billId, userId);

        if (!success)
            return NotFound(ApiResponse<string>
                .FailureResponse(new List<string> { "Bill not found" }));

        return Ok(ApiResponse<string>
            .SuccessResponse("Bill deleted"));
    }
}