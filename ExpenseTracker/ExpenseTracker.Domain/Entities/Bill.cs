using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Entities;

public class Bill
{
    public int BillId { get; private set; }

    [Required]
    public int UserId { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Category { get; private set; }

    [MaxLength(250)]
    public string? Description { get; private set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; private set; }

    [Required]
    [MaxLength(50)]
    public string Frequency { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public DateTime? UpdatedOn { get; private set; }

    private Bill() { }

    public Bill(int userId, string category, string? description,
                decimal amount, string frequency,
                DateTime startDate, DateTime? endDate)
    {
        UserId = userId;
        Category = category;
        Description = description;
        Amount = amount;
        Frequency = frequency;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = true;
        CreatedOn = DateTime.UtcNow;
    }

    public void Update(decimal amount, string category)
    {
        Amount = amount;
        Category = category;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}