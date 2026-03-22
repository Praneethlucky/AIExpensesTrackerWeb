namespace ExpenseTracker.Domain.DTOs.Expenses;

public class ExpenseDto
{
    public int ExpenseId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateTime ExpenseDate { get; set; }
}