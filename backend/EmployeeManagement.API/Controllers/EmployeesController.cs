using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(
    CreateEmployeeDto dto)
    {
        var employee =
            await _employeeService.CreateEmployeeAsync(dto);

        if (employee == null)
        {
            return Conflict("An employee with this email already exists.");
        }

        return CreatedAtAction(
            nameof(GetEmployeeById),
            new { id = employee.UserId },
            employee
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee(
    int id,
    UpdateEmployeeDto dto)
    {
        var result =
            await _employeeService.UpdateEmployeeAsync(id, dto);

        if (result.NotFound)
        {
            return NotFound();
        }

        if (result.EmailConflict)
        {
            return Conflict("An employee with this email already exists.");
        }

        if (result.InvalidRole)
        {
            return BadRequest("Invalid RoleId.");
        }

        if (result.InvalidDepartment)
        {
            return BadRequest("Invalid DepartmentId.");
        }

        return Ok(result.Employee);
    }
}