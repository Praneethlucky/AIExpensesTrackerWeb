using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Entities;

public class User
{
    public int UserId { get; private set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; private set; }

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; private set; }

    [Required]
    public string PasswordHash { get; private set; }

    [Range(0, double.MaxValue)]
    public decimal Salary { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private User() { } // For EF or serialization

    public User(int userId, string fullName, string email, string passwordHash, decimal salary)
    {
        UserId = userId;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Salary = salary;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateSalary(decimal newSalary)
    {
        Salary = newSalary;
        UpdatedAt  = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}