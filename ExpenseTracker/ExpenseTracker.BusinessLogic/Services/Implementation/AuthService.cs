using ExpenseTracker.API.Security;
using ExpenseTracker.BusinessLogic.Exceptions;
using ExpenseTracker.Domain.Configurations;
using ExpenseTracker.Infrastructure.Entities;
using ExpenseTracker.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly TokenService _tokenService;
    private readonly JwtOptions _jwt;

    public AuthService(
        IUserRepository userRepo,
        IRefreshTokenRepository refreshRepo,
        TokenService tokenService,
        IOptions<JwtOptions> jwtOptions)
    {
        _userRepo = userRepo;
        _refreshRepo = refreshRepo;
        _tokenService = tokenService;
        _jwt = jwtOptions.Value;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto request)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email);

        if (user == null)
            throw new UserDoesNotExistsException(request.Email);

        if(!PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.UserId,
            Token = refreshToken,
            DeviceId = request.DeviceId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays),
            Revoked = false
        };

        await _refreshRepo.SaveAsync(refreshEntity);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes)
        };
    }

    public async Task<LoginResponseDto> Refresh(string refreshToken)
    {
        var stored = await _refreshRepo.GetByTokenAsync(refreshToken);

        if (stored == null || stored.Revoked || stored.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Invalid refresh token");

        var user = await _userRepo.GetByIdAsync(stored.UserId);

        var accessToken = _tokenService.GenerateAccessToken(user);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes)
        };
    }

    public async Task Logout(string refreshToken)
    {
        await _refreshRepo.RevokeAsync(refreshToken);
    }

    public async Task LogoutAll(Int32 userId)
    {
        await _refreshRepo.RevokeAllForUser(userId);
    }
}