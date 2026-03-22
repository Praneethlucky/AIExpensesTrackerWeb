using ExpenseTracker.BusinessLogic.DTOs.Reports;
using ExpenseTracker.Domain.Interfaces.Services;

namespace ExpenseTracker.BusinessLogic.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repo;
    private readonly IBillOccurrenceService _occService;

    public ReportService(
        IReportRepository repo,
        IBillOccurrenceService occService)
    {
        _repo = repo;
        _occService = occService;
    }

    public async Task<MonthlyReportDto> GetMonthlyAsync(
        int userId,
        int year,
        int month)
    {
        await _occService.GenerateForMonthAsync(
            userId,
            year,
            month);

        var bills =
            await _repo.GetBillsAsync(
                userId,
                year,
                month);

        var expenses =
            await _repo.GetExpensesAsync(
                userId,
                year,
                month);

        var income =
            await _repo.GetIncomeAsync(
                userId,
                year,
                month);

        decimal totalBills =
            bills.Sum(x => x.Amount);

        decimal totalExpenses =
            expenses.Sum(x => x.Amount);

        decimal remaining =
            income - totalBills - totalExpenses;

        return new MonthlyReportDto
        {
            TotalBills = totalBills,
            TotalExpenses = totalExpenses,
            TotalIncome = income,
            Remaining = remaining
        };
    }
}