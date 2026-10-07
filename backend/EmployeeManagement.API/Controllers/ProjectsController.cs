using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();

        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project == null)
            return NotFound();

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(
    CreateProjectDto dto)
    {
        var project =
            await _projectService.CreateProjectAsync(dto);

        if (project == null)
            return BadRequest("Invalid CreatedBy user.");

        return CreatedAtAction(
            nameof(GetProjectById),
            new { id = project.ProjectId },
            project
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(
    int id,
    UpdateProjectDto dto)
    {
        var project =
            await _projectService.UpdateProjectAsync(id, dto);

        if (project == null)
            return BadRequest("Project not found or invalid CreatedBy user.");

        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var deleted =
            await _projectService.DeleteProjectAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{projectId}/members")]
    public async Task<ActionResult<List<ProjectMemberDto>>> GetProjectMembers(
    int projectId)
    {
        var members =
            await _projectService.GetProjectMembersAsync(projectId);

        return Ok(members);
    }

    [HttpPost("{projectId}/members")]
    public async Task<ActionResult<ProjectMemberDto>> AddProjectMember(
    int projectId,
    AddProjectMemberDto dto)
    {
        var member =
            await _projectService.AddProjectMemberAsync(
                projectId,
                dto
            );

        if (member == null)
        {
            return BadRequest(
                "Unable to add member. Project/user may not exist or user may already be a member."
            );
        }

        return Ok(member);
    }

    [HttpDelete("{projectId}/members/{userId}")]
    public async Task<IActionResult> RemoveProjectMember(
    int projectId,
    int userId)
    {
        var removed =
            await _projectService.RemoveProjectMemberAsync(
                projectId,
                userId
            );

        if (!removed)
            return NotFound();

        return NoContent();
    }
}