public class UserSalaryHistory
{
    public int SalaryHistoryId { get; private set; }

    public int UserId { get; private set; }

    public decimal Salary { get; private set; }

    public DateTime EffectiveFrom { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public UserSalaryHistory(int userId, decimal salary, DateTime effectiveFrom)
    {
        UserId = userId;
        Salary = salary;
        EffectiveFrom = effectiveFrom;
        CreatedAt = DateTime.UtcNow;
    }
}