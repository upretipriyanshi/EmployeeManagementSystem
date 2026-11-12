using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.API.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtService _jwtService;


    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtService jwtService
        )
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _authRepository.GetUserByEmailAsync(
            loginDto.Email
        );

        if (user == null || !user.IsActive)
        {
            return null;
        }

        var verificationResult =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginDto.Password
            );

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.RoleName,
            Token = token
        };
    }

    public async Task<bool> SetupInitialAdminAsync(SetupAdminDto setupDto)
    {
        var user = new User
        {
            FirstName = setupDto.FirstName.Trim(),
            LastName = setupDto.LastName.Trim(),
            Email = setupDto.Email.Trim(),
            IsActive = true
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, setupDto.Password);

        return await _authRepository.CreateInitialAdminAsync(user);
    }
}