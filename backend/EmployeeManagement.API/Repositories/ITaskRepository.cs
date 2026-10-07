using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Repositories;

public interface ITaskRepository
{
    Task<List<ProjectTask>> GetAllAsync();

    Task<ProjectTask?> GetByIdAsync(int id);

    Task<ProjectTask> CreateAsync(ProjectTask task);

    Task<bool> ProjectExistsAsync(int projectId);

    Task<bool> UserExistsAsync(int userId);

    Task<ProjectTask?> GetByIdForUpdateAsync(int id);

    Task SaveChangesAsync();

    Task DeleteAsync(ProjectTask task);
}