using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Infrastructure.Interfaces;

public interface IPaymentTypeRepository
{
    Task<List<PaymentType>> GetAllAsync();

    Task<PaymentType?> GetByIdAsync(int id);
}