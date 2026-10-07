using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;

namespace EmployeeManagement.API.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskDto>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();

        return tasks
            .Select(task => MapToDto(task))
            .ToList();
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null)
            return null;

        return MapToDto(task);
    }

    private static TaskDto MapToDto(ProjectTask task)
    {
        return new TaskDto
        {
            TaskId = task.TaskId,

            ProjectId = task.ProjectId,

            ProjectName = task.Project.ProjectName,

            AssignedTo = task.AssignedTo,

            AssignedToName = task.AssignedUser == null
                ? null
                : $"{task.AssignedUser.FirstName} {task.AssignedUser.LastName}",

            Title = task.Title,

            Description = task.Description,

            Status = task.Status,

            Priority = task.Priority,

            DueDate = task.DueDate,

            CreatedBy = task.CreatedBy,

            CreatedByName =
                $"{task.Creator.FirstName} {task.Creator.LastName}",

            CreatedAt = task.CreatedAt,

            UpdatedAt = task.UpdatedAt
        };
    }

    public async Task<TaskDto?> CreateTaskAsync(CreateTaskDto dto)
    {
        // 1. Check whether the project exists
        var projectExists = await _taskRepository.ProjectExistsAsync(dto.ProjectId);

        if (!projectExists)
            return null;

        // 2. Check whether the creator exists
        var creatorExists = await _taskRepository.UserExistsAsync(dto.CreatedBy);

        if (!creatorExists)
            return null;

        // 3. If task is assigned to someone, check whether that user exists
        if (dto.AssignedTo.HasValue)
        {
            var assignedUserExists =
                await _taskRepository.UserExistsAsync(dto.AssignedTo.Value);

            if (!assignedUserExists)
                return null;
        }

        // 4. Convert CreateTaskDto into ProjectTask entity
        var task = new ProjectTask
        {
            ProjectId = dto.ProjectId,
            AssignedTo = dto.AssignedTo,
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            CreatedBy = dto.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        // 5. Save it into the database
        var createdTask = await _taskRepository.CreateAsync(task);

        // 6. Reload it with navigation properties
        var completeTask = await _taskRepository.GetByIdAsync(createdTask.TaskId);

        if (completeTask == null)
            return null;

        // 7. Convert entity into response DTO
        return MapToDto(completeTask);
    }

    public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        // 1. Find the task we want to update
        var task = await _taskRepository.GetByIdForUpdateAsync(id);

        if (task == null)
            return null;

        // 2. Validate ProjectId
        var projectExists =
            await _taskRepository.ProjectExistsAsync(dto.ProjectId);

        if (!projectExists)
            return null;

        // 3. Validate CreatedBy
        var creatorExists =
            await _taskRepository.UserExistsAsync(dto.CreatedBy);

        if (!creatorExists)
            return null;

        // 4. Validate AssignedTo only when provided
        if (dto.AssignedTo.HasValue)
        {
            var assignedUserExists =
                await _taskRepository.UserExistsAsync(dto.AssignedTo.Value);

            if (!assignedUserExists)
                return null;
        }

        // 5. Update the tracked entity
        task.ProjectId = dto.ProjectId;
        task.AssignedTo = dto.AssignedTo;
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.CreatedBy = dto.CreatedBy;
        task.UpdatedAt = DateTime.UtcNow;

        // 6. Save changes
        await _taskRepository.SaveChangesAsync();

        // 7. Reload with navigation properties
        var updatedTask = await _taskRepository.GetByIdAsync(id);

        if (updatedTask == null)
            return null;

        // 8. Entity -> TaskDto
        return MapToDto(updatedTask);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdForUpdateAsync(id);

        if (task == null)
            return false;

        await _taskRepository.DeleteAsync(task);

        return true;
    }
}