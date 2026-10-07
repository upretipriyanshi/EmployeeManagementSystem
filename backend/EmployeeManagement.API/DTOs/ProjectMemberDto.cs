namespace EmployeeManagement.API.DTOs;

public class ProjectMemberDto
{
    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public DateTime JoinedAt { get; set; }
}