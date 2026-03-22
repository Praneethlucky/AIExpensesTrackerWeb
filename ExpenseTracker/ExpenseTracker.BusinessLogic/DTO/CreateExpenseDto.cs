namespace ExpenseTracker.Domain.DTOs.Expenses;

public class CreateExpenseDto
{
    public int? OccurrenceId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateTime ExpenseDate { get; set; }
}