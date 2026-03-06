
namespace ExpenseTracker.BusinessLogic.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetDashboardAsync(int userId, int year, int month);
}