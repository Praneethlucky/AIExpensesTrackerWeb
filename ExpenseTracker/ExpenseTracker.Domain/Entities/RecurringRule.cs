namespace ExpenseTracker.Domain.Entities;

public class RecurringRule
{
    public int RuleId { get; set; }

    public int BillId { get; set; }

    public string Frequency { get; set; }

    public DateTime StartDate { get; set; }

    public int? DayOfMonth { get; set; }

    public int? DayOfWeek { get; set; }

    public int? MonthOfYear { get; set; }

    public DateTime? EndDate { get; set; }
    public DateTime? NextRunDate { get; set; }

    public bool IsActive { get; set; }
}