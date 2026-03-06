namespace ExpenseTracker.Domain.Interfaces;

public interface IAIProvider
{
    Task<T> GenerateAsync<T>(object input);
}