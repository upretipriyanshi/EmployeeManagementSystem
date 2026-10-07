using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByIdForUpdateAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsForOtherUserAsync(string email, int userId)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && u.UserId != userId);
    }

    public async Task<bool> RoleExistsAsync(int roleId)
    {
        return await _context.Roles
            .AnyAsync(r => r.RoleId == roleId);
    }

    public async Task<bool> DepartmentExistsAsync(int departmentId)
    {
        return await _context.Departments
            .AnyAsync(d => d.DepartmentId == departmentId);
    }
}