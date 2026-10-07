namespace EmployeeManagement.API.Models;

public class ProjectTask
{
    public int TaskId { get; set; }

    public int ProjectId { get; set; }

    public int? AssignedTo { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }


    // Navigation properties

    public Project Project { get; set; } = null!;

    public User? AssignedUser { get; set; }

    public User Creator { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; }
        = new List<Comment>();
}