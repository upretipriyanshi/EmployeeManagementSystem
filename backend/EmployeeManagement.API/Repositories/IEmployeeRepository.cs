using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Repositories;

public interface IEmployeeRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);

    Task<bool> EmailExistsAsync(string email);
    Task<User> CreateAsync(User user);

    Task<User?> GetByIdForUpdateAsync(int id);
    Task SaveChangesAsync();

    Task<bool> EmailExistsForOtherUserAsync(string email, int userId);

    Task<bool> RoleExistsAsync(int roleId);
    Task<bool> DepartmentExistsAsync(int departmentId);
}