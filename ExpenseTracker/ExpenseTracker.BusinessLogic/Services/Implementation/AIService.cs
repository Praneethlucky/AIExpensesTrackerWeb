    using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.Infrastructure.Interfaces;

public class AIService : IAIService
{
    private readonly IBillRepository _billRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAIProvider _aiProvider;

    public AIService(
        IBillRepository billRepository,
        IUserRepository userRepository,
        IAIProvider aiProvider)
    {
        _billRepository = billRepository;
        _userRepository = userRepository;
        _aiProvider = aiProvider;
    }

    public async Task<MonthlyInsightResponseDto> GenerateMonthlyInsightAsync(
        MonthlyInsightRequestDto request)
    {
        throw new NotImplementedException();
    }
}