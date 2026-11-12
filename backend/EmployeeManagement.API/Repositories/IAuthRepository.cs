using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Repositories;

public interface IAuthRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> CreateInitialAdminAsync(User user);
}