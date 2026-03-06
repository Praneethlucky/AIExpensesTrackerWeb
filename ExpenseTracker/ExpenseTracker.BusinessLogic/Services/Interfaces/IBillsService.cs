
namespace ExpenseTracker.BusinessLogic.Interfaces;

public interface IBillService
{
    Task<BillResponseDto> CreateBillAsync(BillCreateDto dto);
    Task<List<BillCreateDto>> GetUserBillsAsync(int userId);
    Task<bool> DeleteBillAsync(int billId, int userId);
}