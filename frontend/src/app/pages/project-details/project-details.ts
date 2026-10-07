import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ProjectService } from '../../services/project';
import { Project } from '../../models/project.model';
import { ProjectMember } from '../../models/project-member.model';
import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/employee.model';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [],
  templateUrl: './project-details.html',
  styleUrl: './project-details.css',
})
export class ProjectDetails implements OnInit {
  employees = signal<Employee[]>([]);
  project = signal<Project | null>(null);
  members = signal<ProjectMember[]>([]);
  selectedUserId = signal<number | null>(null);
  isLoading = signal(true);
  errorMessage = signal('');

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private employeeService: EmployeeService,
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.loadProject(id);
    this.loadProjectMembers(id);
    this.loadEmployees();
  }

  loadProject(id: number): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.projectService.getProjectById(id).subscribe({
      next: (data) => {
        this.project.set(data);
        this.isLoading.set(false);
      },

      error: (error) => {
        console.error('Error loading project:', error);

        this.errorMessage.set('Unable to load project.');

        this.isLoading.set(false);
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/projects']);
  }

  loadProjectMembers(projectId: number): void {
    this.projectService.getProjectMembers(projectId).subscribe({
      next: (data) => {
        this.members.set(data);
      },

      error: (error) => {
        console.error('Error loading project members:', error);
      },
    });
  }

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees.set(data);
      },
      error: (error) => {
        console.error('Error loading employees:', error);
      },
    });
  }

  selectEmployee(value: string): void {
    if (value === '') {
      this.selectedUserId.set(null);
      return;
    }

    this.selectedUserId.set(Number(value));
  }

  addMember(): void {
    const userId = this.selectedUserId();

    const projectId = Number(this.route.snapshot.paramMap.get('id'));

    if (userId === null) {
      return;
    }

    this.projectService.addProjectMember(projectId, { userId }).subscribe({
      next: (createdMember) => {
        this.members.update((currentMembers) => [...currentMembers, createdMember]);

        this.selectedUserId.set(null);
      },

      error: (error) => {
        console.error('Error adding project member:', error);
      },
    });
  }

  removeMember(userId: number): void {
    const projectId = Number(this.route.snapshot.paramMap.get('id'));

    this.projectService.removeProjectMember(projectId, userId).subscribe({
      next: () => {
        this.members.update((currentMembers) =>
          currentMembers.filter((member) => member.userId !== userId),
        );
      },

      error: (error) => {
        console.error('Error removing project member:', error);
      },
    });
  }
}
