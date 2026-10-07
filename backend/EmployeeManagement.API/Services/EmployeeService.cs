using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.API.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPasswordHasher<User> _passwordHasher;


    public EmployeeService(IEmployeeRepository employeeRepository, IPasswordHasher<User> passwordHasher)
    {
        _employeeRepository = employeeRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        var users = await _employeeRepository.GetAllAsync();

        return users.Select(user => MapToDto(user)).ToList();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var user = await _employeeRepository.GetByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        return MapToDto(user);
    }

    public async Task<EmployeeDto?> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var emailExists =
            await _employeeRepository.EmailExistsAsync(dto.Email);

        if (emailExists)
        {
            return null;
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            RoleId = dto.RoleId,
            DepartmentId = dto.DepartmentId,
            IsActive = true

        };

        user.PasswordHash =
            _passwordHasher.HashPassword(user, dto.Password);

        var createdUser =
            await _employeeRepository.CreateAsync(user);

        var userWithDetails =
            await _employeeRepository.GetByIdAsync(createdUser.UserId);

        return MapToDto(userWithDetails!);
    }


    public async Task<UpdateEmployeeResult> UpdateEmployeeAsync(
    int id,
    UpdateEmployeeDto dto)
    {
        var user =
            await _employeeRepository.GetByIdForUpdateAsync(id);

        if (user == null)
        {
            return new UpdateEmployeeResult
            {
                NotFound = true
            };
        }

        var emailExists =
            await _employeeRepository.EmailExistsForOtherUserAsync(
                dto.Email,
                id);

        if (emailExists)
        {
            return new UpdateEmployeeResult
            {
                EmailConflict = true
            };
        }

        var roleExists =
            await _employeeRepository.RoleExistsAsync(dto.RoleId);

        if (!roleExists)
        {
            return new UpdateEmployeeResult
            {
                InvalidRole = true
            };
        }

        if (dto.DepartmentId.HasValue)
        {
            var departmentExists =
                await _employeeRepository.DepartmentExistsAsync(
                    dto.DepartmentId.Value);

            if (!departmentExists)
            {
                return new UpdateEmployeeResult
                {
                    InvalidDepartment = true
                };
            }
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.RoleId = dto.RoleId;
        user.DepartmentId = dto.DepartmentId;
        user.IsActive = dto.IsActive;

        await _employeeRepository.SaveChangesAsync();

        var updatedUser =
            await _employeeRepository.GetByIdAsync(id);

        return new UpdateEmployeeResult
        {
            Employee = MapToDto(updatedUser!)
        };
    }
    private static EmployeeDto MapToDto(User user)
    {
        return new EmployeeDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,

            RoleId = user.RoleId,
            RoleName = user.Role.RoleName,

            DepartmentId = user.DepartmentId,
            DepartmentName = user.Department?.DepartmentName,

            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }

}