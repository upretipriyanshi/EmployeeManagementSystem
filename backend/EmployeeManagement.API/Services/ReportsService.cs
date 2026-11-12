using EmployeeManagement.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Data;

namespace EmployeeManagement.API.Services;

public class ReportsService : IReportsService
{
    private readonly AppDbContext _context;

    public ReportsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReportsViewModel> GetReportSummaryAsync()
    {
        return new ReportsViewModel
        {
            TotalEmployees = await _context.Users.CountAsync(),

            ActiveEmployees = await _context.Users
                .CountAsync(u => u.IsActive),

            TotalProjects = await _context.Projects.CountAsync(),

            TotalTasks = await _context.ProjectTasks.CountAsync(),

            CompletedTasks = await _context.ProjectTasks
                .CountAsync(t => t.Status == "Completed"),

            PendingTasks = await _context.ProjectTasks
                .CountAsync(t => t.Status != "Completed")
        };
    }
}