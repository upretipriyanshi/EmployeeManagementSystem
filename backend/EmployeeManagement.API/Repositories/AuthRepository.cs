using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> CreateInitialAdminAsync(User user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable);

        if (await _context.Users.AnyAsync())
        {
            return false;
        }

        var adminRole = await _context.Roles.SingleOrDefaultAsync(role => role.RoleName == "Admin");
        if (adminRole == null)
        {
            return false;
        }

        user.RoleId = adminRole.RoleId;
        user.Role = adminRole;
        user.IsActive = true;
        user.CreatedAt = DateTime.UtcNow;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }
}