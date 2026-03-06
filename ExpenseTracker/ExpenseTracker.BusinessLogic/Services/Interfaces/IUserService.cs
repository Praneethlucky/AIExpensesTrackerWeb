
namespace ExpenseTracker.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

        Task<bool> RegisterAsync(LoginRequestDto request);
        Task<bool> UpdateSalaryAsync(int userId, decimal salary);
    }
}
