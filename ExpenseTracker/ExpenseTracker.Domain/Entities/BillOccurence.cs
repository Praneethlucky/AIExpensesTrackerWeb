namespace ExpenseTracker.Domain.Entities;

public class BillOccurrence
{
    public int OccurrenceId { get; set; }

    public int BillId { get; set; }

    public int UserId { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public bool IsPaid { get; set; }

    public bool IsActive { get; set; }
    
    public string CategoryName { get; set; }

    public string CategoryIcon { get; set; }
    public string CategoryColor { get; set; }
    public string PaymentTypeName { get; set; }
}