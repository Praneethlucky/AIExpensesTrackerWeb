
namespace ExpenseTracker.BusinessLogic.Interfaces;

public interface IAIService
{
    Task<MonthlyInsightResponseDto> GenerateMonthlyInsightAsync(
        MonthlyInsightRequestDto request);
}