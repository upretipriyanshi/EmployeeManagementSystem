namespace EmployeeManagement.API.DTOs;

public class EmployeeDto
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}