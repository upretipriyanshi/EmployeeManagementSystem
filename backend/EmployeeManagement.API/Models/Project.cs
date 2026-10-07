namespace EmployeeManagement.API.Models;

public class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }


    // Navigation properties

    public User Creator { get; set; } = null!;

    public ICollection<ProjectMember> ProjectMembers { get; set; }
        = new List<ProjectMember>();

    public ICollection<ProjectTask> Tasks { get; set; }
        = new List<ProjectTask>();
}