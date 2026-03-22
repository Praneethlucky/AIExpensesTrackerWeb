namespace ExpenseTracker.Domain.Entities;

public class PaymentType
{
    public int PaymentTypeId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}