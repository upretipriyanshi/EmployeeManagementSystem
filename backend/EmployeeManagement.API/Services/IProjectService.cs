using EmployeeManagement.API.DTOs;

namespace EmployeeManagement.API.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();

    Task<ProjectDto?> GetProjectByIdAsync(int id);

    Task<ProjectDto?> CreateProjectAsync(CreateProjectDto dto);

    Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto dto);

    Task<bool> DeleteProjectAsync(int id);

    Task<List<ProjectMemberDto>> GetProjectMembersAsync(int projectId);

    Task<ProjectMemberDto?> AddProjectMemberAsync(
        int projectId,
        AddProjectMemberDto dto
    );

    Task<bool> RemoveProjectMemberAsync(
        int projectId,
        int userId
    );
}