using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Domain.Interfaces.Services;
using ExpenseTracker.Infrastructure.Interfaces;

public class BillOccurrenceService
    : IBillOccurrenceService
{
    private readonly IBillOccurrenceRepository _occRepo;
    private readonly IRecurringRuleRepository _ruleRepo;
    private readonly IBillRepository _billRepo;

    public BillOccurrenceService(
        IBillOccurrenceRepository occRepo,
        IRecurringRuleRepository ruleRepo,
        IBillRepository billRepo)
    {
        _occRepo = occRepo;
        _ruleRepo = ruleRepo;
        _billRepo = billRepo;
    }

    public async Task GenerateForMonthAsync(
    int userId,
    int year,
    int month)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var rules =
            await _ruleRepo.GetActiveRulesAsync(userId);

        var bills =
            await _billRepo.GetAllAsync(userId);

        foreach (var rule in rules)
        {
            var bill =
                bills.FirstOrDefault(
                    x => x.BillId == rule.BillId);

            if (bill == null)
                continue;

            if (rule.NextRunDate == null)
                continue;

            var next = rule.NextRunDate.Value;

            if (rule.EndDate != null &&
                next > rule.EndDate.Value)
                continue;

            while (next <= end)
            {
                if (next >= start)
                {
                    bool exists =
                        await _occRepo.ExistsAsync(
                            bill.BillId,
                            next);

                    if (!exists)
                    {
                        await _occRepo.InsertAsync(
                            new BillOccurrence
                            {
                                BillId = bill.BillId,
                                UserId = userId,
                                DueDate = next,
                                Amount = bill.Amount
                            });
                    }
                }

                next = GetNextDate(
                    rule.Frequency,
                    next);

                if (rule.EndDate != null &&
                    next > rule.EndDate.Value)
                    break;
            }

            await _ruleRepo.UpdateNextRunAsync(
                rule.RuleId,
                next);
        }
    }

    public async Task<List<BillOccurrence>>
        GetMonthlyAsync(
        int userId,
        int year,
        int month)
    {
        return await _occRepo
            .GetMonthlyAsync(
                userId,
                year,
                month);
    }

    public async Task MarkPaidAsync(
        int occurrenceId, int userId)
    {
        await _occRepo
            .MarkPaidAsync(
                occurrenceId, userId);
    }

    private DateTime GetNextDate(
    string frequency,
    DateTime current)
    {
        switch (frequency)
        {
            case "Weekly":
                return current.AddDays(7);

            case "Monthly":
                return current.AddMonths(1);

            case "Quarterly":
                return current.AddMonths(3);

            case "Yearly":
                return current.AddYears(1);

            default:
                throw new Exception(
                    "Invalid frequency");
        }
    }
}