namespace EmployeeManagement.API.DTOs;

public class TaskDto
{
    public int TaskId { get; set; }

    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public int? AssignedTo { get; set; }

    public string? AssignedToName { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    public int CreatedBy { get; set; }

    public string CreatedByName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}