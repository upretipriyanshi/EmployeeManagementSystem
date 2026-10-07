namespace EmployeeManagement.API.Models;

public class Comment
{
    public int CommentId { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }


    // Navigation properties

    public ProjectTask Task { get; set; } = null!;

    public User User { get; set; } = null!;
}