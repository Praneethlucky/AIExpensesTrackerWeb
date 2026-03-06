using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Entities;

public class AIInsight
{
    public int InsightId { get; private set; }

    [Required]
    public int UserId { get; private set; }

    public int Year { get; private set; }

    public int Month { get; private set; }

    [Required]
    public string InsightJson { get; private set; }

    public DateTime GeneratedOn { get; private set; }

    private AIInsight() { }

    public AIInsight(int userId, int year, int month, string insightJson)
    {
        UserId = userId;
        Year = year;
        Month = month;
        InsightJson = insightJson;
        GeneratedOn = DateTime.UtcNow;
    }
}