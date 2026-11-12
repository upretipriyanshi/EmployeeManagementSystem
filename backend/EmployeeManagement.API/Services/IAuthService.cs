using EmployeeManagement.API.DTOs;

namespace EmployeeManagement.API.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> SetupInitialAdminAsync(SetupAdminDto setupDto);
}