using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();

    Task<Project?> GetByIdAsync(int id);

    Task<Project> CreateAsync(Project project);

    Task<bool> UserExistsAsync(int userId);

    Task<Project?> GetByIdForUpdateAsync(int id);

    Task SaveChangesAsync();

    Task DeleteAsync(Project project);

    Task<List<ProjectMember>> GetProjectMembersAsync(int projectId);

    Task<ProjectMember?> GetProjectMemberAsync(
        int projectId,
        int userId
    );

    Task<ProjectMember> AddProjectMemberAsync(
        ProjectMember projectMember
    );

    Task DeleteProjectMemberAsync(
        ProjectMember projectMember
    );
}