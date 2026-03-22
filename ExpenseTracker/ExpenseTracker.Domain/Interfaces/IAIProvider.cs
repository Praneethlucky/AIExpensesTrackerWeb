namespace ExpenseTracker.Infrastructure.Interfaces;

public interface IAIProvider
{
    Task<T> GenerateAsync<T>(object input);
}