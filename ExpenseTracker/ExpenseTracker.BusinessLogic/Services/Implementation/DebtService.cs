using ExpenseTracker.BusinessLogic.DTO;
using ExpenseTracker.BusinessLogic.Services.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Services.Implementation
{
    public class DebtService : IDebtService
    {
        private readonly IDebtRepository _repo;

        public DebtService(IDebtRepository repo)
        {
            _repo = repo;
        }

        public async Task CreateDebt(int userId, CreateDebtDto dto)
        {
            var debt = new Debt
            {
                UserId = userId,
                PersonName = dto.PersonName,
                Type = dto.Type,
                TotalAmount = dto.Amount,
                Description = dto.Description
            };

            await _repo.CreateDebt(debt);
        }


        public async Task AddPayment(int userId, AddDebtPaymentDto dto)
        {
            var p = new DebtPayment
            {
                UserId = userId,
                DebtId = dto.DebtId,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate
            };

            await _repo.AddPayment(p);
        }


        public Task<List<Debt>> GetDebts(int userId)
            => _repo.GetDebts(userId);


        public async Task<DebtSummaryDto> GetSummary(int userId)
        {
            var e = await _repo.GetSummary(userId);

            return new DebtSummaryDto
            {
                GivenPending = e.GivenPending,
                TakenPending = e.TakenPending
            };
        }
    }
}
