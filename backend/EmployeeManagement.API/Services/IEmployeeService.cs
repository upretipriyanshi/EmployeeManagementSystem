using EmployeeManagement.API.DTOs;

namespace EmployeeManagement.API.Services;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllEmployeesAsync();

    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);

    Task<EmployeeDto?> CreateEmployeeAsync(CreateEmployeeDto dto);

    Task<UpdateEmployeeResult?> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
}