using ExpenseTracker.Domain.Entities;

public interface IBillOccurrenceService
{
    Task GenerateForMonthAsync(
        int userId,
        int year,
        int month);

    Task<List<BillOccurrence>>
        GetMonthlyAsync(
        int userId,
        int year,
        int month);

    Task MarkPaidAsync(
        int occurrenceId, int userId);
}