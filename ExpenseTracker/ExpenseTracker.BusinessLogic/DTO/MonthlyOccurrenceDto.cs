namespace ExpenseTracker.BusinessLogic.DTOs.Occurrences;

public class MonthlyOccurrencesDto
{
    public List<OccurrenceItemDto> upcoming { get; set; }

    public List<OccurrenceItemDto> paid { get; set; }

    public List<OccurrenceItemDto> pending { get; set; }

    public decimal totalBills { get; set; }

    public decimal totalPaid { get; set; }

    public decimal totalSalary { get; set; }

    public decimal totalIncome { get; set; }

    public decimal remaining { get; set; }
}