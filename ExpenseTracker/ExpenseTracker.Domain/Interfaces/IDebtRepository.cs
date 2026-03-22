using ExpenseTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface IDebtRepository
    {
        Task<int> CreateDebt(Debt debt);
        Task AddPayment(DebtPayment payment);
        Task<List<Debt>> GetDebts(int userId);
        Task<DebtSummary> GetSummary(int userId);
    }
}
