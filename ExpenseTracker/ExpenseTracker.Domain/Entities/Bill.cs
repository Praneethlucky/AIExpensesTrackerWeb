namespace ExpenseTracker.Domain.Entities;

public class Bill
{
    public int BillId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public int PaymentTypeId { get; set; }
    public string Frequency { get; set; }

    public bool IsActive { get; set; }
}