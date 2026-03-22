namespace ExpenseTracker.BusinessLogic.DTOs.Bills;

public class CreateBillDto
{
    public string Name { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public int PaymentTypeId { get; set; }

    public string Frequency { get; set; }

    public DateTime StartDate { get; set; }

    public int? DayOfMonth { get; set; }

    public int? DayOfWeek { get; set; }

    public int? MonthOfYear { get; set; }

    public DateTime? EndDate { get; set; }
}