using ExpenseTracker.Domain.Entities;

public interface IExpenseRepository
{
    Task<int> InsertAsync(Expense expense);

    Task UpdateAsync(Expense expense);

    Task DeleteAsync(int expenseId, int userId);

    Task<List<Expense>> GetMonthlyAsync(
        int userId,
        int year,
        int month);

    Task MarkOccurrencePaidAsync(int occurrenceId);
}