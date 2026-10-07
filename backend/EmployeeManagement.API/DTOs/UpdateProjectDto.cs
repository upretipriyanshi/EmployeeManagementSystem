using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs;

public class UpdateProjectDto
{
    [Required]
    [MaxLength(150)]
    public string ProjectName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [Required]
    public int CreatedBy { get; set; }
}