namespace EmployeeManagement.API.Models;

public class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public int? DepartmentId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }


    // Navigation properties

    public Role Role { get; set; } = null!;

    public Department? Department { get; set; }
}