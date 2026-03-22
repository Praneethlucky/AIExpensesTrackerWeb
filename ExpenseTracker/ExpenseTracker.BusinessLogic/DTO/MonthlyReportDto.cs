namespace ExpenseTracker.BusinessLogic.DTOs.Reports;

public class MonthlyReportDto
{
    public decimal TotalBills { get; set; }

    public decimal TotalExpenses { get; set; }

    public decimal TotalIncome { get; set; }

    public decimal Remaining { get; set; }
}