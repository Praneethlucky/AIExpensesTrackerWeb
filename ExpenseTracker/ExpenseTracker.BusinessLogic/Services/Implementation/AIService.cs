    using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.Domain.Interfaces;

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
        var bills = await _billRepository
            .GetMonthlyBillsAsync(request.UserId, request.Year, request.Month);

        var user = await _userRepository
            .GetByIdAsync(request.UserId);

        var total = bills.Sum(x => x.Amount);

        var aiResult = await _aiProvider
                                        .GenerateAsync<MonthlyInsightResponseDto>(new
                                        {
                                            Salary = user.Salary,
                                            Total = total
                                        });

        return aiResult;
    }
}