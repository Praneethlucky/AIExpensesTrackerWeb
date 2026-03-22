using ExpenseTracker.BusinessLogic.DTOs.Occurrences;

public interface IOccurrenceQueryService
{
    Task<MonthlyOccurrencesDto>
        GetMonthlyAsync(
        int userId,
        int year,
        int month);
    Task MarkPaidAsync(
        int occurrenceId, int userId);
}
