namespace ExpenseTracker.Domain.Entities;

public class Expense
{
    public int ExpenseId { get; set; }

    public int UserId { get; set; }

    public int? OccurrenceId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateTime ExpenseDate { get; set; }

    public bool IsActive { get; set; }
}