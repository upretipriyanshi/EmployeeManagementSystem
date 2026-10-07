using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;

namespace EmployeeManagement.API.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();

        return projects
            .Select(project => MapToDto(project))
            .ToList();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project == null)
            return null;

        return MapToDto(project);
    }

    public async Task<ProjectDto?> CreateProjectAsync(CreateProjectDto dto)
    {
        // Check whether the creator exists
        var userExists =
            await _projectRepository.UserExistsAsync(dto.CreatedBy);

        if (!userExists)
            return null;

        // Convert CreateProjectDto → Project entity
        var project = new Project
        {
            ProjectName = dto.ProjectName,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            CreatedBy = dto.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        // INSERT into database
        var createdProject =
            await _projectRepository.CreateAsync(project);

        // Fetch again so Creator navigation property is loaded
        var projectWithCreator =
            await _projectRepository.GetByIdAsync(createdProject.ProjectId);

        return MapToDto(projectWithCreator!);
    }

    public async Task<ProjectDto?> UpdateProjectAsync(
        int id,
        UpdateProjectDto dto)
    {
        // Find the existing project
        var project =
            await _projectRepository.GetByIdForUpdateAsync(id);

        if (project == null)
            return null;

        // Check whether CreatedBy is a valid user
        var userExists =
            await _projectRepository.UserExistsAsync(dto.CreatedBy);

        if (!userExists)
            return null;

        // Update the tracked entity
        project.ProjectName = dto.ProjectName;
        project.Description = dto.Description;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.Status = dto.Status;
        project.CreatedBy = dto.CreatedBy;

        // Save changes to SQL Server
        await _projectRepository.SaveChangesAsync();

        // Fetch again with Creator included
        var updatedProject =
            await _projectRepository.GetByIdAsync(id);

        return MapToDto(updatedProject!);
    }
    private static ProjectDto MapToDto(Project project)
    {
        return new ProjectDto
        {
            ProjectId = project.ProjectId,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            CreatedBy = project.CreatedBy,
            CreatedByName =
                $"{project.Creator.FirstName} {project.Creator.LastName}",
            CreatedAt = project.CreatedAt
        };
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project =
            await _projectRepository.GetByIdForUpdateAsync(id);

        if (project == null)
            return false;

        await _projectRepository.DeleteAsync(project);

        return true;
    }

    public async Task<List<ProjectMemberDto>> GetProjectMembersAsync(
    int projectId)
    {
        var members =
            await _projectRepository.GetProjectMembersAsync(projectId);

        return members
            .Select(member => MapMemberToDto(member))
            .ToList();
    }

    private static ProjectMemberDto MapMemberToDto(
    ProjectMember member)
    {
        return new ProjectMemberDto
        {
            ProjectId = member.ProjectId,
            UserId = member.UserId,
            FirstName = member.User.FirstName,
            LastName = member.User.LastName,
            Email = member.User.Email,
            RoleName = member.User.Role.RoleName,
            JoinedAt = member.JoinedAt
        };
    }

    public async Task<ProjectMemberDto?> AddProjectMemberAsync(
    int projectId,
    AddProjectMemberDto dto)
    {
        // Check project exists
        var project =
            await _projectRepository.GetByIdAsync(projectId);

        if (project == null)
            return null;

        // Check user exists
        var userExists =
            await _projectRepository.UserExistsAsync(dto.UserId);

        if (!userExists)
            return null;

        // Check whether this relationship already exists
        var existingMember =
            await _projectRepository.GetProjectMemberAsync(
                projectId,
                dto.UserId
            );

        if (existingMember != null)
            return null;

        var projectMember = new ProjectMember
        {
            ProjectId = projectId,
            UserId = dto.UserId,
            JoinedAt = DateTime.UtcNow
        };

        await _projectRepository.AddProjectMemberAsync(projectMember);

        var members =
            await _projectRepository.GetProjectMembersAsync(projectId);

        var createdMember =
            members.First(member => member.UserId == dto.UserId);

        return MapMemberToDto(createdMember);
    }

    public async Task<bool> RemoveProjectMemberAsync(
    int projectId,
    int userId)
    {
        var projectMember =
            await _projectRepository.GetProjectMemberAsync(
                projectId,
                userId
            );

        if (projectMember == null)
            return false;

        await _projectRepository
            .DeleteProjectMemberAsync(projectMember);

        return true;
    }
}