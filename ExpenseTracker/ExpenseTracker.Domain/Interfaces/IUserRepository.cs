namespace ExpenseTracker.Domain.Interfaces;

using ExpenseTracker.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int userId);
    Task<bool> InsertAsync(User user);
    Task<bool> UpdateSalaryAsync(int userId, decimal salary);
}