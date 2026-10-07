mkdir EmployeeManagementSystem
--frontend
cd EmployeeManagementSystem
node -v
npm -v
ng version
npm install -g @angular/cli
ng new frontend --routing --style=css
cd frontend
ng serve
--backend
cd ..
dotnet --version
mkdir backend
cd backend
dotnet new webapi -n EmployeeManagement.API
cd EmployeeManagement.API
--sql
dotnet run
docker --version
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=YourStrongPassword123! -p 1433:1433 --name employee-sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
docker ps

--run
docker start employee-sqlserver
cd EmployeeManagementSystem/backend/EmployeeManagement.API
dotnet run
cd EmployeeManagementSystem/frontend
ng serve

--when docker stops running
pkill -f Docker
pkill -f com.docker
ps aux | grep -i docker
kill -9 28495
Don't reuse 28495 in the future—the PID will probably be different.
open -a Docker
docker ps

--generate component
ng generate component layout/navbar
ng generate component layout/sidebar
ng generate component layout/main-layout
ng g c pages/login
ng g c pages/dashboard
ng g c pages/projects
ng g c pages/tasks
ng g c pages/employees

--sql queries
CREATE DATABASE EmployeeManagementDB;
GO

USE EmployeeManagementDB;
GO

CREATE TABLE Roles
(
RoleId INT IDENTITY(1,1) PRIMARY KEY,
RoleName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Departments
(
DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Users
(
UserId INT IDENTITY(1,1) PRIMARY KEY,

    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,

    RoleId INT NOT NULL,
    DepartmentId INT NULL,

    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Users_Roles
        FOREIGN KEY (RoleId)
        REFERENCES Roles(RoleId),

    CONSTRAINT FK_Users_Departments
        FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId)

);

CREATE TABLE Projects
(
ProjectId INT IDENTITY(1,1) PRIMARY KEY,

    ProjectName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,

    StartDate DATE NULL,
    EndDate DATE NULL,

    Status NVARCHAR(30) NOT NULL DEFAULT 'Not Started',

    CreatedBy INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Projects_CreatedBy
        FOREIGN KEY (CreatedBy)
        REFERENCES Users(UserId)

);

CREATE TABLE ProjectMembers
(
ProjectId INT NOT NULL,
UserId INT NOT NULL,
JoinedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_ProjectMembers
        PRIMARY KEY (ProjectId, UserId),

    CONSTRAINT FK_ProjectMembers_Projects
        FOREIGN KEY (ProjectId)
        REFERENCES Projects(ProjectId),

    CONSTRAINT FK_ProjectMembers_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)

);

CREATE TABLE ProjectTasks
(
TaskId INT IDENTITY(1,1) PRIMARY KEY,

    ProjectId INT NOT NULL,
    AssignedTo INT NULL,

    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,

    Status NVARCHAR(30) NOT NULL DEFAULT 'To Do',
    Priority NVARCHAR(20) NOT NULL DEFAULT 'Medium',

    DueDate DATETIME2 NULL,

    CreatedBy INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_ProjectTasks_Projects
        FOREIGN KEY (ProjectId)
        REFERENCES Projects(ProjectId),

    CONSTRAINT FK_ProjectTasks_AssignedTo
        FOREIGN KEY (AssignedTo)
        REFERENCES Users(UserId),

    CONSTRAINT FK_ProjectTasks_CreatedBy
        FOREIGN KEY (CreatedBy)
        REFERENCES Users(UserId)

);

CREATE TABLE Comments
(
CommentId INT IDENTITY(1,1) PRIMARY KEY,

    TaskId INT NOT NULL,
    UserId INT NOT NULL,

    CommentText NVARCHAR(1000) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Comments_Tasks
        FOREIGN KEY (TaskId)
        REFERENCES ProjectTasks(TaskId),

    CONSTRAINT FK_Comments_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)

);

CREATE TABLE AuditLogs
(
AuditLogId BIGINT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NULL,

    Action NVARCHAR(100) NOT NULL,
    EntityName NVARCHAR(100) NULL,
    EntityId INT NULL,

    Details NVARCHAR(MAX) NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_AuditLogs_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)

);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

--debugged
GET /api/employees
↓
404
↓
Is server running?
↓
YES
↓
Is correct project running?
↓
YES
↓
Does controller file exist?
↓
YES
↓
Is controller compiled into DLL?
↓
YES
↓
Does ASP.NET discover controller?
↓
YES
↓
Does ASP.NET discover controller ACTIONS?
↓
NO ❌
↓
Inspect actual controller file
↓
EMPTY EmployeesController
↓
ROOT CAUSE FOUND
