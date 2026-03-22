using ExpenseTracker.Domain.Entities;

public interface IOccurrenceQueryRepository
{
    Task<List<BillOccurrence>> GetOccurrencesAsync(
        int userId,
        int year,
        int month);

    Task<decimal> GetExpensesTotalAsync(
        int userId,
        int year,
        int month);

    Task<decimal> GetIncomeTotalAsync(
        int userId,
        int year,
        int month);

    Task<decimal> GetSalaryAsync(
        int userId,
        int year,
        int month);

    Task<string> GetBillNameAsync(
        int billId);
}