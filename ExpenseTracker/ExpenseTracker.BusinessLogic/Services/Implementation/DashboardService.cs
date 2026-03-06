using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.BusinessLogic.Interfaces;

public class DashboardService : IDashboardService
{
    private readonly IBillRepository _billRepository;
    private readonly IUserRepository _userRepository;

    public DashboardService(
        IBillRepository billRepository,
        IUserRepository userRepository)
    {
        _billRepository = billRepository;
        _userRepository = userRepository;
    }

    public async Task<DashboardSummaryDto> GetDashboardAsync(
        int userId, int year, int month)
    {
        var bills = await _billRepository.GetMonthlyBillsAsync(userId, year, month);
        var user = await _userRepository.GetByIdAsync(userId);

        var total = bills.Sum(x => x.Amount);

        return new DashboardSummaryDto
        {
            Salary = user.Salary,
            TotalExpenses = total,
            PredictedNextMonth = total * 1.05m,
            RiskLevel = total > user.Salary ? "High" : "Normal"
        };
    }
}