import { Component, OnInit, inject, signal } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  selectAllTasks,
  selectTaskLoading,
  selectTaskError,
  selectTotalCount,
  selectPageNumber,
  selectPageSize,
  selectTotalPages,
} from '../../store/task/task.selectors';
import {
  loadTasks,
  createTask,
  updateTask,
  deleteTask,
  filterTasks,
} from '../../store/task/task.action';
import { AsyncPipe } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormsModule,
} from '@angular/forms';
import { Task } from '../../models/task.model';

import { map } from 'rxjs';
import { selectUserRole } from '../../store/auth/auth.selectors';
import { selectAuthUserId } from '../../store/auth/auth.selectors';
import { ProjectService } from '../../services/project';
import { EmployeeService } from '../../services/employee';
import { Project } from '../../models/project.model';
import { Employee } from '../../models/employee.model';
import { take } from 'rxjs';

@Component({
  selector: 'app-tasks',
  imports: [AsyncPipe, ReactiveFormsModule, FormsModule],
  templateUrl: './tasks.html',
  styleUrl: './tasks.css',
})
export class Tasks implements OnInit {
  editingTaskId: number | null = null;
  private store = inject(Store);
  private fb = inject(FormBuilder);
  private projectService = inject(ProjectService);
  private employeeService = inject(EmployeeService);

  projects = signal<Project[]>([]);
  employees = signal<Employee[]>([]);
  currentUserId = signal<number | null>(null);

  canManageTasks$ = this.store
    .select(selectUserRole)
    .pipe(map((role) => role === 'Admin' || role === 'Manager'));
  isAdmin$ = this.store.select(selectUserRole).pipe(map((role) => role === 'Admin'));
  private editingTaskCreatorId: number | null = null;

  taskForm: FormGroup = this.fb.group({
    projectId: [null, Validators.required],
    assignedTo: [null],
    title: ['', Validators.required],
    description: [''],
    status: ['To Do', Validators.required],
    priority: ['Medium', Validators.required],
    dueDate: [null],
  });

  searchText = '';
  statusFilter = '';
  priorityFilter = '';

  tasks$ = this.store.select(selectAllTasks);
  loading$ = this.store.select(selectTaskLoading);
  error$ = this.store.select(selectTaskError);

  totalCount$ = this.store.select(selectTotalCount);
  pageNumber$ = this.store.select(selectPageNumber);
  pageSize$ = this.store.select(selectPageSize);
  totalPages$ = this.store.select(selectTotalPages);

  sortBy = 'createdAt';
  sortDirection = 'desc';
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    this.loadPagedTasks();
    this.store
      .select(selectAuthUserId)
      .pipe(take(1))
      .subscribe((userId) => this.currentUserId.set(userId));
    this.projectService.getProjects().subscribe({
      next: (projects) => this.projects.set(projects),
      error: () =>
        this.store.dispatch(
          filterTasks({
            search: this.searchText,
            status: this.statusFilter,
            priority: this.priorityFilter,
            sortBy: this.sortBy,
            sortDirection: this.sortDirection,
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,
          }),
        ),
    });
    this.employeeService.getEmployees().subscribe({
      next: (employees) => this.employees.set(employees),
    });
  }

  onSubmit(): void {
    if (this.taskForm.invalid) {
      return;
    }

    const formValue = this.taskForm.value;
    const currentUserId = this.currentUserId();
    if (currentUserId === null) {
      return;
    }

    const taskData = {
      projectId: Number(formValue.projectId),
      assignedTo: formValue.assignedTo ? Number(formValue.assignedTo) : null,
      title: formValue.title,
      description: formValue.description || null,
      status: formValue.status,
      priority: formValue.priority,
      dueDate: formValue.dueDate || null,
      createdBy: this.editingTaskCreatorId ?? currentUserId,
    };

    if (this.editingTaskId === null) {
      this.store.dispatch(
        createTask({
          task: taskData,
        }),
      );
    } else {
      this.store.dispatch(
        updateTask({
          id: this.editingTaskId,
          task: taskData,
        }),
      );
    }
    this.resetForm();
  }

  editTask(task: Task): void {
    this.editingTaskId = task.taskId;
    this.editingTaskCreatorId = task.createdBy;

    this.taskForm.patchValue({
      projectId: task.projectId,
      assignedTo: task.assignedTo,
      title: task.title,
      description: task.description,
      status: task.status,
      priority: task.priority,
      dueDate: task.dueDate ? task.dueDate.substring(0, 10) : null,
      createdBy: task.createdBy,
    });
  }

  resetForm(): void {
    this.editingTaskId = null;
    this.editingTaskCreatorId = null;

    this.taskForm.reset({
      projectId: null,
      assignedTo: null,
      title: '',
      description: '',
      status: 'To Do',
      priority: 'Medium',
      dueDate: null,
    });
  }

  removeTask(id: number): void {
    if (window.confirm('Delete this task? This action cannot be undone.')) {
      this.store.dispatch(deleteTask({ id }));
    }
  }

  applyFilters(): void {
    this.pageNumber = 1;
    this.loadPagedTasks();
  }

  loadPagedTasks(): void {
    this.store.dispatch(
      filterTasks({
        search: this.searchText,
        status: this.statusFilter,
        priority: this.priorityFilter,
        sortBy: this.sortBy,
        sortDirection: this.sortDirection,
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
      }),
    );
  }

  nextPage(totalPages: number): void {
    if (this.pageNumber < totalPages) {
      this.pageNumber++;
      this.loadPagedTasks();
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadPagedTasks();
    }
  }

  changePageSize(): void {
    this.pageNumber = 1;
    this.loadPagedTasks();
  }

  clearFilters(): void {
    this.searchText = '';
    this.statusFilter = '';
    this.priorityFilter = '';
    this.sortBy = 'createdAt';
    this.sortDirection = 'desc';
    this.pageNumber = 1;
    this.pageSize = 5;

    this.loadPagedTasks();
  }
}
