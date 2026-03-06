using System.ComponentModel.DataAnnotations;

public class MonthlyInsightRequestDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int Month { get; set; }
}