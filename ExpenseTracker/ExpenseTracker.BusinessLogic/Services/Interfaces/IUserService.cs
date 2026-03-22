
using ExpenseTracker.Infrastructure.Entities;

namespace ExpenseTracker.BusinessLogic.Interfaces
{
    public interface IUserService
    {

        Task<int> RegisterAsync(RegisterUserRequestDTO user);
        Task<bool> UpdateSalaryAsync(int userId, decimal salary);
        Task<UserProfileResponseDTO> GetUserProfileByID(int userId);
        Task<UserProfileResponseDTO> GetUserProfileByEmail(string email);

    }
}
