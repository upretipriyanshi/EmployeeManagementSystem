import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Project, CreateProject, UpdateProject } from '../models/project.model';
import { ProjectMember, AddProjectMember } from '../models/project-member.model';
import { API_BASE_URL } from '../config/api-base-url';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private apiUrl = `${API_BASE_URL}/api/Projects`;

  constructor(private http: HttpClient) {}

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.apiUrl);
  }

  deleteProject(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateProject(id: number, project: UpdateProject): Observable<Project> {
    return this.http.put<Project>(`${this.apiUrl}/${id}`, project);
  }

  createProject(project: CreateProject): Observable<Project> {
    return this.http.post<Project>(this.apiUrl, project);
  }

  getProjectById(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.apiUrl}/${id}`);
  }

  getProjectMembers(projectId: number): Observable<ProjectMember[]> {
    return this.http.get<ProjectMember[]>(`${this.apiUrl}/${projectId}/members`);
  }

  addProjectMember(projectId: number, member: AddProjectMember): Observable<ProjectMember> {
    return this.http.post<ProjectMember>(`${this.apiUrl}/${projectId}/members`, member);
  }

  removeProjectMember(projectId: number, userId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${projectId}/members/${userId}`);
  }
}
