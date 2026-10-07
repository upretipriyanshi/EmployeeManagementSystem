using EmployeeManagement.API.DTOs;

namespace EmployeeManagement.API.Services;

public class UpdateEmployeeResult
{
    public EmployeeDto? Employee { get; set; }

    public bool NotFound { get; set; }

    public bool EmailConflict { get; set; }

    public bool InvalidRole { get; set; }

    public bool InvalidDepartment { get; set; }
}