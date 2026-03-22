using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.BusinessLogic.Services;

public class RecurringRuleService : IRecurringRuleService
{
    private readonly IRecurringRuleRepository _repo;

    public RecurringRuleService(IRecurringRuleRepository repo)
    {
        _repo = repo;
    }

    public async Task CreateAsync(RecurringRule rule)
    {
        await _repo.InsertAsync(rule);
    }

    public async Task UpdateAsync(RecurringRule rule)
    {
        await _repo.UpdateAsync(rule);
    }

    public async Task DeleteAsync(int billId)
    {
        await _repo.DeleteAsync(billId);
    }

    public async Task<RecurringRule?> GetByBillIdAsync(int billId)
    {
        return await _repo.GetByBillIdAsync(billId);
    }

    public async Task<List<RecurringRule>> GetActiveRulesAsync(int userId)
    {
        return await _repo.GetActiveRulesAsync(userId);
    }
}