namespace EmployeeManagement.API.DTOs;

public class ProjectDto
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public int CreatedBy { get; set; }

    public string CreatedByName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}