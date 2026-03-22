using AutoMapper;
using ExpenseTracker.Infrastructure.Interfaces;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.API.Security;
using ExpenseTracker.Infrastructure.Entities;
using ExpenseTracker.BusinessLogic.Exceptions;

namespace ExpenseTracker.BusinessLogic.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> RegisterAsync(RegisterUserRequestDTO user)
    {
        var existingUser = await GetUserProfileByEmail(user.Email);
        if(existingUser != null)
        {
            throw new UserAlreadyExistsException(user.Email);
        }
        var mappedUser = _mapper.Map<User>(user);
        var registeredUser = await _repository.InsertAsync(mappedUser);
        UpdateSalaryAsync(registeredUser, user.CurrentSalary);
        return registeredUser;
    }

    public async Task<bool> UpdateSalaryAsync(int userId, decimal salary)
    {
        var user = await _repository.GetByIdAsync(userId);

        user.UpdateSalary(salary);

        UserSalaryHistory history = new UserSalaryHistory(
                                                            userId,
                                                            salary,
                                                            DateTime.UtcNow.Date
                                                            );
        await _repository.AddSalaryHistory(history);

        return await _repository.UpdateSalaryAsync(userId, salary);
    }

    public async Task<UserProfileResponseDTO> GetUserProfileByID(int userId)
    {
        var user = await _repository.GetByIdAsync(userId);

        return _mapper.Map<UserProfileResponseDTO>(user);
    }
    public async Task<UserProfileResponseDTO> GetUserProfileByEmail(string email)
    {
        var user = await _repository.GetByEmailAsync(email);

        return _mapper.Map<UserProfileResponseDTO>(user);
    }

}