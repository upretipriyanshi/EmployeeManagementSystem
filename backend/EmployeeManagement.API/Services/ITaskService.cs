using EmployeeManagement.API.DTOs;

namespace EmployeeManagement.API.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllTasksAsync();

    Task<TaskDto?> GetTaskByIdAsync(int id);

    Task<TaskDto?> CreateTaskAsync(CreateTaskDto dto);

    Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto dto);

    Task<bool> DeleteTaskAsync(int id);

}