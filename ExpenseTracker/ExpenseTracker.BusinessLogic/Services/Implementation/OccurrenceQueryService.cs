using ExpenseTracker.BusinessLogic.DTOs.Occurrences;

public class OccurrenceQueryService
    : IOccurrenceQueryService
{
    private readonly IOccurrenceQueryRepository _repo;
    private readonly IBillOccurrenceService _gen;

    public OccurrenceQueryService(
        IOccurrenceQueryRepository repo,
        IBillOccurrenceService gen)
    {
        _repo = repo;
        _gen = gen;
    }
    public async Task MarkPaidAsync(
        int occurrenceId, int userId)
    {
        await _gen.MarkPaidAsync(occurrenceId, userId);
    }
    public async Task<MonthlyOccurrencesDto>
        GetMonthlyAsync(
        int userId,
        int year,
        int month)
    {
        await _gen.GenerateForMonthAsync(
            userId, year, month);

        var occ =
            await _repo.GetOccurrencesAsync(
                userId, year, month);

        var upcoming = new List<OccurrenceItemDto>();
        var paid = new List<OccurrenceItemDto>();

        foreach (var o in occ)
        {
            var name =
                await _repo.GetBillNameAsync(
                    o.BillId);

            var dto =
                new OccurrenceItemDto
                {
                    occurrenceId = o.OccurrenceId,
                    name = name,
                    amount = o.Amount,
                    dueDate = o.DueDate,
                    isPaid = o.IsPaid,
                    categoryName = o.CategoryName,
                    categoryColor = o.CategoryColor,
                    categoryIcon = o.CategoryIcon,
                    paymentTypeName = o.PaymentTypeName
                };

            if (o.IsPaid)
                paid.Add(dto);
            else
                upcoming.Add(dto);
        }

        var salary =
    await _repo.GetSalaryAsync(
        userId, year, month);

        var income =
            await _repo.GetIncomeTotalAsync(
                userId, year, month);

        var totalIncome =
            salary + income;

        var expenses =
            await _repo.GetExpensesTotalAsync(
                userId, year, month);

        var totalBills =
            occ.Sum(x => x.Amount);

        var remaining =
            totalIncome
            - totalBills
            - expenses;

        return new MonthlyOccurrencesDto
        {
            upcoming = upcoming,
            paid = paid,
            pending = upcoming,

            totalBills = totalBills,
            totalPaid = paid.Sum(x => x.amount),

            totalSalary = salary,
            totalIncome = totalIncome,

            remaining = remaining
        };
    }
    
}