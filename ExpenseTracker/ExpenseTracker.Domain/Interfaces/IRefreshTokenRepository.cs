using ExpenseTracker.Infrastructure.Entities;

namespace ExpenseTracker.Infrastructure.Interfaces;

public interface IRefreshTokenRepository
{
    Task SaveAsync(RefreshToken token);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task RevokeAsync(string token);

    Task RevokeAllForUser(Int32 userId);
}