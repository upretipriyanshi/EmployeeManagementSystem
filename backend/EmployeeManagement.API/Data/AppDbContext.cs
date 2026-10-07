using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Data;

public class AppDbContext : DbContext
{
    // =====================================================
    // CONSTRUCTOR
    // =====================================================

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    // =====================================================
    // DBSETS
    // =====================================================

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }


    // =====================================================
    // MODEL CONFIGURATION
    // =====================================================

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // =====================================================
        // PRIMARY KEYS
        // =====================================================

        // Composite Primary Key:
        // ProjectId + UserId
        modelBuilder.Entity<ProjectMember>()
            .HasKey(pm => new { pm.ProjectId, pm.UserId });

        // ProjectTask doesn't follow EF's default PK naming convention
        // because the property is TaskId instead of ProjectTaskId.
        modelBuilder.Entity<ProjectTask>()
            .HasKey(t => t.TaskId);


        // =====================================================
        // RELATIONSHIPS
        // =====================================================

        // ---------------------
        // User -> Role
        // ---------------------

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // User -> Department
        // ---------------------

        modelBuilder.Entity<User>()
            .HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // Project -> Creator
        // ---------------------

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // ProjectMember -> Project
        // ---------------------

        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.ProjectMembers)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // ProjectMember -> User
        // ---------------------

        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.User)
            .WithMany()
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // ProjectTask -> Project
        // ---------------------

        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // ProjectTask -> Assigned User
        // ---------------------

        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.AssignedUser)
            .WithMany()
            .HasForeignKey(t => t.AssignedTo)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // ProjectTask -> Creator
        // ---------------------

        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.Creator)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // Comment -> ProjectTask
        // ---------------------

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // Comment -> User
        // ---------------------

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        // ---------------------
        // AuditLog -> User
        // ---------------------

        modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        // =====================================================
        // ROLE CONFIGURATION
        // =====================================================

        modelBuilder.Entity<Role>()
            .Property(r => r.RoleName)
            .HasMaxLength(50);

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.RoleName)
            .IsUnique();


        // =====================================================
        // DEPARTMENT CONFIGURATION
        // =====================================================

        modelBuilder.Entity<Department>()
            .Property(d => d.DepartmentName)
            .HasMaxLength(100);

        modelBuilder.Entity<Department>()
            .HasIndex(d => d.DepartmentName)
            .IsUnique();

        modelBuilder.Entity<Department>()
            .Property(d => d.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        // =====================================================
        // USER CONFIGURATION
        // =====================================================

        modelBuilder.Entity<User>()
            .Property(u => u.FirstName)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(u => u.LastName)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(150);

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .HasMaxLength(255);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.IsActive)
            .HasDefaultValue(true);

        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        // =====================================================
        // PROJECT CONFIGURATION
        // =====================================================

        modelBuilder.Entity<Project>()
            .Property(p => p.ProjectName)
            .HasMaxLength(150);

        modelBuilder.Entity<Project>()
            .Property(p => p.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Project>()
            .Property(p => p.Status)
            .HasMaxLength(30)
            .HasDefaultValue("Not Started");

        modelBuilder.Entity<Project>()
            .Property(p => p.StartDate)
            .HasColumnType("date");

        modelBuilder.Entity<Project>()
            .Property(p => p.EndDate)
            .HasColumnType("date");

        modelBuilder.Entity<Project>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        // =====================================================
        // PROJECT MEMBER CONFIGURATION
        // =====================================================

        modelBuilder.Entity<ProjectMember>()
            .Property(pm => pm.JoinedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        // =====================================================
        // PROJECT TASK CONFIGURATION
        // =====================================================

        modelBuilder.Entity<ProjectTask>()
            .Property(t => t.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<ProjectTask>()
            .Property(t => t.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<ProjectTask>()
            .Property(t => t.Status)
            .HasMaxLength(30)
            .HasDefaultValue("To Do");

        modelBuilder.Entity<ProjectTask>()
            .Property(t => t.Priority)
            .HasMaxLength(20)
            .HasDefaultValue("Medium");

        modelBuilder.Entity<ProjectTask>()
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        // =====================================================
        // COMMENT CONFIGURATION
        // =====================================================

        modelBuilder.Entity<Comment>()
            .Property(c => c.CommentText)
            .HasMaxLength(1000);

        modelBuilder.Entity<Comment>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        // =====================================================
        // AUDIT LOG CONFIGURATION
        // =====================================================

        modelBuilder.Entity<AuditLog>()
            .Property(a => a.Action)
            .HasMaxLength(100);

        modelBuilder.Entity<AuditLog>()
            .Property(a => a.EntityName)
            .HasMaxLength(100);

        modelBuilder.Entity<AuditLog>()
            .Property(a => a.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        // =====================================================
        // SEED DATA - ROLES
        // =====================================================

        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 1,
                RoleName = "Admin"
            },
            new Role
            {
                RoleId = 2,
                RoleName = "Manager"
            },
            new Role
            {
                RoleId = 3,
                RoleName = "Employee"
            }
        );


        // =====================================================
        // SEED DATA - DEPARTMENTS
        // =====================================================

        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                DepartmentId = 1,
                DepartmentName = "Engineering",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Department
            {
                DepartmentId = 2,
                DepartmentName = "Human Resources",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Department
            {
                DepartmentId = 3,
                DepartmentName = "Finance",
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );
    }
}