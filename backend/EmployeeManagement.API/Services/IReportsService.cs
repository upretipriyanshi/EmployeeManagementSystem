using EmployeeManagement.API.ViewModels;

namespace EmployeeManagement.API.Services;

public interface IReportsService
{
    Task<ReportsViewModel> GetReportSummaryAsync();
}