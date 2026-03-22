using ExpenseTracker.Infrastructure.Interfaces;
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
        var user = await _userRepository.GetByIdAsync(userId);


        return new DashboardSummaryDto
        {
            Salary = user.CurrentSalary
            
        };
    }
}