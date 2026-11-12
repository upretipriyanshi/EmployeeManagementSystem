import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/paged-result.model';

import { Task, CreateTask, UpdateTask } from '../models/task.model';
import { API_BASE_URL } from '../config/api-base-url';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private apiUrl = `${API_BASE_URL}/api/Tasks`;

  constructor(private http: HttpClient) {}

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl);
  }

  createTask(task: CreateTask): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task);
  }

  updateTask(id: number, task: UpdateTask): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${id}`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  searchTasks(
    search: string,
    status: string,
    priority: string,
    sortBy: string,
    sortDirection: string,
    pageNumber: number,
    pageSize: number,
  ): Observable<PagedResult<Task>> {
    let params = new HttpParams();

    if (search.trim()) {
      params = params.set('search', search.trim());
    }

    if (status) {
      params = params.set('status', status);
    }

    if (priority) {
      params = params.set('priority', priority);
    }

    if (sortBy) {
      params = params.set('sortBy', sortBy);
    }

    if (sortDirection) {
      params = params.set('sortDirection', sortDirection);
    }

    params = params.set('pageNumber', pageNumber.toString());
    params = params.set('pageSize', pageSize.toString());

    return this.http.get<PagedResult<Task>>(`${this.apiUrl}/search`, { params });
  }
}
