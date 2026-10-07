namespace EmployeeManagement.API.Models;

public class AuditLog
{
    public long AuditLogId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? EntityName { get; set; }

    public int? EntityId { get; set; }

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; }


    // Navigation property

    public User? User { get; set; }
}