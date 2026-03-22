namespace ExpenseTracker.Domain.DTOs.Bills;

public class UpdateBillDto
{
    public int BillId { get; set; }

    public string Name { get; set; }

    public decimal Amount { get; set; }

    public int PaymentTypeId { get; set; }
    public int CategoryId { get; set; }
    public string Frequency { get; set; }
}