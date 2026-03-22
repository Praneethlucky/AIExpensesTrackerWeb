
using ExpenseTracker.BusinessLogic.DTOs.Bills;
using ExpenseTracker.Domain.DTOs.Bills;

namespace ExpenseTracker.BusinessLogic.Interfaces;

public interface IBillService
{
    Task<int> CreateAsync(int userId, CreateBillDto dto);

    Task<List<BillDto>> GetAllAsync(int userId);

    Task UpdateAsync(int userId, UpdateBillDto dto);

    Task DeleteAsync(int userId, int billId);
}