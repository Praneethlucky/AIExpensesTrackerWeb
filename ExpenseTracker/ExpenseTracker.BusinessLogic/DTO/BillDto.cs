namespace ExpenseTracker.BusinessLogic.DTOs.Bills;

public class BillDto
{
    public int BillId { get; set; }

    public string Name { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public int PaymentTypeId { get; set; }

    public string Frequency { get; set; }
}