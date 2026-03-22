using ExpenseTracker.Domain.DTOs.PaymentTypes;

namespace ExpenseTracker.Domain.Interfaces.Services;

public interface IPaymentTypeService
{
    Task<List<PaymentTypeDto>> GetAll();

    Task<PaymentTypeDto> GetById(int id);
}