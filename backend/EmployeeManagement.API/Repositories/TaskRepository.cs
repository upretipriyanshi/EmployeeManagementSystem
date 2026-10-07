using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectTask>> GetAllAsync()
    {
        return await _context.ProjectTasks
            .Include(task => task.Project)
            .Include(task => task.AssignedUser)
            .Include(task => task.Creator)
            .ToListAsync();
    }

    public async Task<ProjectTask?> GetByIdAsync(int id)
    {
        return await _context.ProjectTasks
            .Include(task => task.Project)
            .Include(task => task.AssignedUser)
            .Include(task => task.Creator)
            .FirstOrDefaultAsync(task => task.TaskId == id);
    }

    public async Task<ProjectTask> CreateAsync(ProjectTask task)
    {
        await _context.ProjectTasks.AddAsync(task);

        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<bool> ProjectExistsAsync(int projectId)
    {
        return await _context.Projects
            .AnyAsync(project => project.ProjectId == projectId);
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users
            .AnyAsync(user => user.UserId == userId);
    }

    public async Task<ProjectTask?> GetByIdForUpdateAsync(int id)
    {
        return await _context.ProjectTasks
            .FirstOrDefaultAsync(task => task.TaskId == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProjectTask task)
    {
        _context.ProjectTasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}