using ExpenseTracker.BusinessLogic.Exceptions;
using ExpenseTracker.Domain.DTOs.PaymentTypes;
using ExpenseTracker.Domain.Interfaces.Services;
using ExpenseTracker.Infrastructure.Interfaces;

namespace ExpenseTracker.BusinessLogic.Services;

public class PaymentTypeService : IPaymentTypeService
{
    private readonly IPaymentTypeRepository _repo;

    public PaymentTypeService(IPaymentTypeRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<PaymentTypeDto>> GetAll()
    {
        var list = await _repo.GetAllAsync();

        return list.Select(x => new PaymentTypeDto
        {
            PaymentTypeId = x.PaymentTypeId,
            Name = x.Name
        }).ToList();
    }

    public async Task<PaymentTypeDto> GetById(int id)
    {
        var item = await _repo.GetByIdAsync(id);

        if (item == null)
            throw new NotFoundException("Payment type not found");

        return new PaymentTypeDto
        {
            PaymentTypeId = item.PaymentTypeId,
            Name = item.Name
        };
    }
}