using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs;

public class UpdateEmployeeDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }

    public int? DepartmentId { get; set; }

    public bool IsActive { get; set; }
}