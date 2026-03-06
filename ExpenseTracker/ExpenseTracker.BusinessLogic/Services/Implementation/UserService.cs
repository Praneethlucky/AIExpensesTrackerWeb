using AutoMapper;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.API.Security;

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

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _repository.GetByEmailAsync(request.Email);

        if (user == null)
            return null;

        // Example password validation
        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        return _mapper.Map<LoginResponseDto>(user);
    }

    public async Task<bool> RegisterAsync(LoginRequestDto request)
    {
        return false; // Registration logic to be implemented
    }

    public async Task<bool> UpdateSalaryAsync(int userId, decimal salary)
    {
        return await _repository.UpdateSalaryAsync(userId, salary);
    }
}