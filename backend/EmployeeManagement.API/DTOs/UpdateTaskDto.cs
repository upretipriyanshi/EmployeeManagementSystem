using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs;

public class UpdateTaskDto
{
    [Required]
    public int ProjectId { get; set; }

    public int? AssignedTo { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Priority { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    [Required]
    public int CreatedBy { get; set; }
}