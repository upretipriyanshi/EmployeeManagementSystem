import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api-base-url';

@Injectable({
  providedIn: 'root',
})
export class Reports {
  private readonly http = inject(HttpClient);

  private readonly reportsUrl = `${API_BASE_URL}/Reports/Index`;

  getReportsPage(): Observable<string> {
    return this.http.get(this.reportsUrl, {
      responseType: 'text',
    });
  }
}
