using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Creator)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Creator)
            .FirstOrDefaultAsync(p => p.ProjectId == id);
    }

    public async Task<Project> CreateAsync(Project project)
    {
        await _context.Projects.AddAsync(project);

        await _context.SaveChangesAsync();

        return project;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users
            .AnyAsync(user => user.UserId == userId);
    }

    public async Task<Project?> GetByIdForUpdateAsync(int id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.ProjectId == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProjectMember>> GetProjectMembersAsync(
    int projectId)
    {
        return await _context.ProjectMembers
            .Where(pm => pm.ProjectId == projectId)
            .Include(pm => pm.User)
                .ThenInclude(user => user.Role)
            .ToListAsync();
    }

    public async Task<ProjectMember?> GetProjectMemberAsync(
    int projectId,
    int userId)
    {
        return await _context.ProjectMembers
            .FirstOrDefaultAsync(pm =>
                pm.ProjectId == projectId &&
                pm.UserId == userId
            );
    }

    public async Task<ProjectMember> AddProjectMemberAsync(
    ProjectMember projectMember)
    {
        await _context.ProjectMembers.AddAsync(projectMember);

        await _context.SaveChangesAsync();

        return projectMember;
    }

    public async Task DeleteProjectMemberAsync(
    ProjectMember projectMember)
    {
        _context.ProjectMembers.Remove(projectMember);

        await _context.SaveChangesAsync();
    }
}