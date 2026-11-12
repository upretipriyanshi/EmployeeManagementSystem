using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET: /api/Tasks
    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();

        return Ok(tasks);
    }

    // GET: /api/Tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto dto)
    {
        var createdTask = await _taskService.CreateTaskAsync(dto);

        if (createdTask == null)
        {
            return BadRequest(
                "Unable to create task. Project or user may not exist."
            );
        }

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = createdTask.TaskId },
            createdTask
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TaskDto>> UpdateTask(
    int id,
    UpdateTaskDto dto)
    {
        var updatedTask = await _taskService.UpdateTaskAsync(id, dto);

        if (updatedTask == null)
        {
            return BadRequest(
                "Unable to update task. Task, project, or user may not exist."
            );
        }

        return Ok(updatedTask);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResult<TaskDto>>> SearchTasks(
     [FromQuery] string? search,
     [FromQuery] string? status,
     [FromQuery] string? priority,
     [FromQuery] string? sortBy,
     [FromQuery] string? sortDirection,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 5)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 5;
        }

        var result = await _taskService.SearchAsync(
            search,
            status,
            priority,
            sortBy,
            sortDirection,
            pageNumber,
            pageSize
        );

        return Ok(result);
    }
}