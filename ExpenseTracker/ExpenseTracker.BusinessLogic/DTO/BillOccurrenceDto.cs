namespace ExpenseTracker.Domain.DTOs.Reports;

public class BillOccurrenceDto
{
    public int OccurrenceId { get; set; }

    public int BillId { get; set; }

    public decimal Amount { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsPaid { get; set; }
}