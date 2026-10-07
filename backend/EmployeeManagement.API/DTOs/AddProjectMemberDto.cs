using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs;

public class AddProjectMemberDto
{
    [Required]
    public int UserId { get; set; }
}