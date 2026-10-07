namespace EmployeeManagement.API.Models;

public class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}