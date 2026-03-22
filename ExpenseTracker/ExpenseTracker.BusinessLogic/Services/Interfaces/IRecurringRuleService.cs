using ExpenseTracker.Domain.Entities;

public interface IRecurringRuleService
{
    Task CreateAsync(RecurringRule rule);

    Task UpdateAsync(RecurringRule rule);

    Task DeleteAsync(int billId);

    Task<RecurringRule?> GetByBillIdAsync(int billId);

    Task<List<RecurringRule>> GetActiveRulesAsync(int userId);
}