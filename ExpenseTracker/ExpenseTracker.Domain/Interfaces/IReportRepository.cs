using ExpenseTracker.Domain.Entities;

public interface IReportRepository
{
    Task<List<BillOccurrence>> GetBillsAsync(
        int userId,
        int year,
        int month);

    Task<List<Expense>> GetExpensesAsync(
        int userId,
        int year,
        int month);

    Task<decimal> GetIncomeAsync(
        int userId,
        int year,
        int month);
}