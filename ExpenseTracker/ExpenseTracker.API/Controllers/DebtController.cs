using Azure;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.DTO;
using ExpenseTracker.BusinessLogic.Services.Interfaces;
using ExpenseTracker.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/debts")]
    public class DebtController : BaseController
    {
        private readonly IDebtService _service;

        public DebtController(IDebtService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateDebtDto dto)
        {

            await _service.CreateDebt(UserId, dto);

            return Ok(ApiResponse<object>.SuccessResponse("Created Debt successfully"));
        }


        [HttpPost("payment")]
        public async Task<IActionResult> AddPayment(AddDebtPaymentDto dto)
        {

            await _service.AddPayment(UserId, dto);

            return Ok(ApiResponse<object>.SuccessResponse("Logged Payment successfully"));
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {

            var data = await _service.GetDebts(UserId);

            return Ok(ApiResponse<object>.SuccessResponse(data, "Fetched list successfully"));
        }


        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {

            var data = await _service.GetSummary(UserId);

            return Ok(ApiResponse<object>.SuccessResponse(data, "Fetched Summary successfully"));
        }
    }
}
