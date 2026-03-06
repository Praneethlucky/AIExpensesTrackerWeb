using System.ComponentModel.DataAnnotations;

public class BillCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Category { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public string Frequency { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}