using ExpenseTracker.Domain.Entities;

public interface IRecurringRuleRepository
{
    Task InsertAsync(RecurringRule rule);

    Task UpdateAsync(RecurringRule rule);

    Task DeleteAsync(int billId);

    Task<RecurringRule?> GetByBillIdAsync(int billId);

    Task<List<RecurringRule>> GetActiveRulesAsync(int userId);
    Task UpdateNextRunAsync(int ruleId, DateTime nextRunDate);
}