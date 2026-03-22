namespace ExpenseTracker.Infrastructure.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; }

    public string DeviceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool Revoked { get; set; }

    public string? ReplacedByToken { get; set; }
}