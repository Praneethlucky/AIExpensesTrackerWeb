namespace ExpenseTracker.Infrastructure.Interfaces;

using ExpenseTracker.Infrastructure.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int userId);
    Task<int> InsertAsync(User user);
    Task<bool> UpdateSalaryAsync(int userId, decimal salary);
    Task<bool> AddSalaryHistory(UserSalaryHistory history);


}