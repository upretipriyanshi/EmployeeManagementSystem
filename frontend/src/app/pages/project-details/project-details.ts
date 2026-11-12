import { Component, OnInit, signal, computed, ElementRef, ViewChild } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { ProjectService } from '../../services/project';
import { Project } from '../../models/project.model';
import { ProjectMember } from '../../models/project-member.model';
import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/employee.model';
import { Store } from '@ngrx/store';
import { inject } from '@angular/core';
import { map, take } from 'rxjs';

import { selectUserRole } from '../../store/auth/auth.selectors';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './project-details.html',
  styleUrl: './project-details.css',
})
export class ProjectDetails implements OnInit {
  private store = inject(Store);

  memberSuccessMessage = signal('');
  memberErrorMessage = signal('');
  removingMemberId = signal<number | null>(null);
  isAddingMember = signal(false);
  employees = signal<Employee[]>([]);
  project = signal<Project | null>(null);
  members = signal<ProjectMember[]>([]);
  selectedUserId = signal<number | null>(null);
  isLoading = signal(true);
  errorMessage = signal('');

  @ViewChild('employeeSelect')
  employeeSelect?: ElementRef<HTMLSelectElement>;

  readonly userRole$;
  readonly canManageMembers$;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private employeeService: EmployeeService,
  ) {
    this.userRole$ = this.store.select(selectUserRole);

    this.canManageMembers$ = this.userRole$.pipe(
      map((role) => role === 'Admin' || role === 'Manager'),
    );
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.loadProject(id);
    this.loadProjectMembers(id);
    this.canManageMembers$.pipe(take(1)).subscribe((canManage) => {
      if (canManage) {
        this.loadEmployees();
      }
    });
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
    this.canManageMembers$.pipe(take(1)).subscribe((canManage) => {
      if (!canManage) {
        return;
      }

      const userId = this.selectedUserId();

      const projectId = Number(this.route.snapshot.paramMap.get('id'));

      if (userId === null) {
        return;
      }

      if (this.isAddingMember()) {
        return;
      }

      this.isAddingMember.set(true);

      this.projectService.addProjectMember(projectId, { userId }).subscribe({
        next: (createdMember) => {
          this.members.update((currentMembers) => [...currentMembers, createdMember]);

          this.selectedUserId.set(null);
          if (this.employeeSelect) {
            this.employeeSelect.nativeElement.value = '';
          }
          this.isAddingMember.set(false);
          this.memberErrorMessage.set('');
          this.memberSuccessMessage.set('Member added successfully.');
        },

        error: (error) => {
          console.error('Error adding project member:', error);
          this.memberSuccessMessage.set('');
          this.memberErrorMessage.set('Unable to add member. Please try again.');
        },
      });
    });
  }

  removeMember(userId: number): void {
    this.canManageMembers$.pipe(take(1)).subscribe((canManage) => {
      if (!canManage) {
        return;
      }

      if (this.removingMemberId() !== null) {
        return;
      }

      const projectId = Number(this.route.snapshot.paramMap.get('id'));

      this.removingMemberId.set(userId);

      this.projectService.removeProjectMember(projectId, userId).subscribe({
        next: () => {
          this.members.update((currentMembers) =>
            currentMembers.filter((member) => member.userId !== userId),
          );

          this.removingMemberId.set(null);
          this.memberErrorMessage.set('');
          this.memberSuccessMessage.set('Member removed successfully.');
        },

        error: (error) => {
          console.error('Error removing project member:', error);
          this.removingMemberId.set(null);
          this.memberSuccessMessage.set('');
          this.memberErrorMessage.set('Unable to remove member. Please try again.');
        },
      });
    });
  }

  availableEmployees = computed(() => {
    const existingMemberIds = new Set(this.members().map((member) => member.userId));

    return this.employees().filter((employee) => !existingMemberIds.has(employee.userId));
  });
}
