using ExpenseTracker.Domain.DTOs.Expenses;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Domain.Interfaces.Services;

namespace ExpenseTracker.BusinessLogic.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _repo;

    public ExpenseService(IExpenseRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> CreateAsync(
        int userId,
        CreateExpenseDto dto)
    {
        var expense = new Expense
        {
            UserId = userId,
            Amount = dto.Amount,
            Description = dto.Description,
            ExpenseDate = dto.ExpenseDate,
            OccurrenceId = dto.OccurrenceId
        };

        var id = await _repo.InsertAsync(expense);

        if (dto.OccurrenceId.HasValue)
        {
            await _repo.MarkOccurrencePaidAsync(
                dto.OccurrenceId.Value);
        }

        return id;
    }

    public async Task UpdateAsync(
        int userId,
        ExpenseDto dto)
    {
        await _repo.UpdateAsync(new Expense
        {
            ExpenseId = dto.ExpenseId,
            UserId = userId,
            Amount = dto.Amount,
            Description = dto.Description,
            ExpenseDate = dto.ExpenseDate
        });
    }

    public async Task DeleteAsync(
        int userId,
        int expenseId)
    {
        await _repo.DeleteAsync(expenseId, userId);
    }

    public async Task<List<ExpenseDto>> GetMonthlyAsync(
        int userId,
        int year,
        int month)
    {
        var list = await _repo.GetMonthlyAsync(
            userId,
            year,
            month);

        return list.Select(x => new ExpenseDto
        {
            ExpenseId = x.ExpenseId,
            Amount = x.Amount,
            Description = x.Description,
            ExpenseDate = x.ExpenseDate
        }).ToList();
    }
}