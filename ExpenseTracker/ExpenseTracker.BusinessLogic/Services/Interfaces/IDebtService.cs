using ExpenseTracker.BusinessLogic.DTO;
using ExpenseTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Services.Interfaces
{
    public interface IDebtService
    {
        Task CreateDebt(int userId, CreateDebtDto dto);

        Task AddPayment(int userId, AddDebtPaymentDto dto);

        Task<List<Debt>> GetDebts(int userId);

        Task<DebtSummaryDto> GetSummary(int userId);
    }
}
