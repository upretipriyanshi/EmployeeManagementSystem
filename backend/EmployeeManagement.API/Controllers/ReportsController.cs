using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.Services;

namespace EmployeeManagement.API.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class ReportsController : Controller
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var report = await _reportsService.GetReportSummaryAsync();

        return View(report);
    }
}