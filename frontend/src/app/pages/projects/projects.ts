import { Component, OnInit, signal } from '@angular/core';
import { ProjectService } from '../../services/project';
import { Project, CreateProject, UpdateProject } from '../../models/project.model';
import { Router } from '@angular/router';

import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './projects.html',
  styleUrl: './projects.css',
})
export class Projects implements OnInit {
  projects = signal<Project[]>([]);
  isLoading = signal(true);
  errorMessage = signal('');

  projectForm!: FormGroup;
  showForm = signal(false);
  editingProjectId = signal<number | null>(null);

  constructor(
    private projectService: ProjectService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.loadProjects();
  }

  loadProjects(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.projectService.getProjects().subscribe({
      next: (data) => {
        this.projects.set(data);
        this.isLoading.set(false);
      },

      error: (error) => {
        console.error('Error loading projects:', error);
        this.errorMessage.set('Unable to load projects.');
        this.isLoading.set(false);
      },
    });
  }

  initializeForm(): void {
    this.projectForm = this.formBuilder.group({
      projectName: ['', [Validators.required, Validators.maxLength(150)]],

      description: ['', Validators.maxLength(1000)],

      startDate: [null],

      endDate: [null],

      status: ['', [Validators.required, Validators.maxLength(50)]],

      createdBy: [null, Validators.required],
    });
  }

  deleteProject(id: number): void {
    this.projectService.deleteProject(id).subscribe({
      next: () => {
        this.projects.update((currentProjects) =>
          currentProjects.filter((project) => project.projectId !== id),
        );
      },

      error: (error) => {
        console.error('Error deleting project:', error);
      },
    });
  }

  editProject(project: Project): void {
    this.editingProjectId.set(project.projectId);

    this.projectForm.patchValue({
      projectName: project.projectName,
      description: project.description,
      startDate: project.startDate,
      endDate: project.endDate,
      status: project.status,
      createdBy: project.createdBy,
    });

    this.showForm.set(true);
  }

  onSubmit(): void {
    if (this.projectForm.invalid) {
      this.projectForm.markAllAsTouched();
      return;
    }

    const projectId = this.editingProjectId();
    const formValue = this.projectForm.getRawValue();

    if (projectId !== null) {
      // EDIT
      const updatedProject: UpdateProject = formValue;

      this.projectService.updateProject(projectId, updatedProject).subscribe({
        next: (projectFromBackend) => {
          this.projects.update((currentProjects) =>
            currentProjects.map((project) =>
              project.projectId === projectFromBackend.projectId ? projectFromBackend : project,
            ),
          );

          this.closeForm();
        },

        error: (error) => {
          console.error('Error updating project:', error);
        },
      });
    } else {
      // CREATE
      const newProject: CreateProject = formValue;

      this.projectService.createProject(newProject).subscribe({
        next: (createdProject) => {
          this.projects.update((currentProjects) => [...currentProjects, createdProject]);

          this.closeForm();
        },

        error: (error) => {
          console.error('Error creating project:', error);
        },
      });
    }
  }

  closeForm(): void {
    this.projectForm.reset();
    this.editingProjectId.set(null);
    this.showForm.set(false);
  }

  cancelForm(): void {
    this.closeForm();
  }

  showProjectForm(): void {
    this.editingProjectId.set(null);
    this.projectForm.reset();
    this.showForm.set(true);
  }

  viewProject(id: number): void {
    this.router.navigate(['/projects', id]);
  }
}
