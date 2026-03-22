namespace ExpenseTracker.Domain.DTOs.Reports;

public class ExpenseReportDto
{
    public int ExpenseId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateTime ExpenseDate { get; set; }
}