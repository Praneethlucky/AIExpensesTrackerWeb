using ExpenseTracker.Domain.DTOs.Expenses;

public interface IExpenseService
{
    Task<int> CreateAsync(
        int userId,
        CreateExpenseDto dto);

    Task UpdateAsync(
        int userId,
        ExpenseDto dto);

    Task DeleteAsync(
        int userId,
        int expenseId);

    Task<List<ExpenseDto>> GetMonthlyAsync(
        int userId,
        int year,
        int month);
}