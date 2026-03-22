using ExpenseTracker.BusinessLogic.DTOs.Reports;

public interface IReportService
{
    Task<MonthlyReportDto> GetMonthlyAsync(
        int userId,
        int year,
        int month);
}