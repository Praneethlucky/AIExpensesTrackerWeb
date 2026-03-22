using ExpenseTracker.Domain.Entities;

public interface IBillRepository
{
    Task<bool> ExistsAsync(
        int userId,
        string name);

    Task<int> InsertBillAsync(
        Bill bill);

    Task InsertRuleAsync(
        RecurringRule rule);

    Task<List<Bill>> GetAllAsync(
        int userId);

    Task UpdateAsync(
        Bill bill);

    Task DeleteAsync(
        int billId,
        int userId);
}