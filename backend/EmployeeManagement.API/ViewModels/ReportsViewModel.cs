namespace EmployeeManagement.API.ViewModels;

public class ReportsViewModel
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int TotalProjects { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int PendingTasks { get; set; }
}