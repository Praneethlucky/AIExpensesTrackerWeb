using ExpenseTracker.Domain.Entities;

public interface IBillOccurrenceRepository
{
    Task<bool> ExistsAsync(
        int billId,
        DateTime dueDate);

    Task InsertAsync(
        BillOccurrence occurrence);

    Task<List<BillOccurrence>> GetMonthlyAsync(
        int userId,
        int year,
        int month);

    Task MarkPaidAsync(
        int occurrenceId, int userId);

    Task<List<BillOccurrence>> GetUnpaidAsync(
        int userId);
}