using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.DTOs;

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

    public async Task<PagedResult<ProjectTask>> SearchAsync(
    string? search,
    string? status,
    string? priority,
    string? sortBy,
    string? sortDirection,
    int pageNumber,
    int pageSize)
    {
        var query = _context.ProjectTasks
            .Include(t => t.Project)
            .Include(t => t.AssignedUser)
            .Include(t => t.Creator)
            .AsQueryable();

        // FILTERING
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t =>
                t.Title.Contains(search) ||
                (t.Description != null &&
                 t.Description.Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(t => t.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            query = query.Where(t => t.Priority == priority);
        }

        // COUNT AFTER FILTERING, BEFORE PAGINATION
        var totalCount = await query.CountAsync();

        // SORTING
        var descending =
            sortDirection?.ToLower() == "desc";

        query = sortBy?.ToLower() switch
        {
            "title" => descending
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title),

            "duedate" => descending
                ? query.OrderByDescending(t => t.DueDate)
                : query.OrderBy(t => t.DueDate),

            "priority" => descending
                ? query.OrderByDescending(t => t.Priority)
                : query.OrderBy(t => t.Priority),

            "status" => descending
                ? query.OrderByDescending(t => t.Status)
                : query.OrderBy(t => t.Status),

            _ => descending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt)
        };

        query = ((IOrderedQueryable<ProjectTask>)query)
            .ThenBy(t => t.TaskId);

        // PAGINATION
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(
            (double)totalCount / pageSize
        );

        return new PagedResult<ProjectTask>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
}