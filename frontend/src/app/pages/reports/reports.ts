import { Component, inject, signal } from '@angular/core';
import { Reports } from '../../services/reports';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [],
  templateUrl: './reports.html',
  styleUrl: './reports.css',
})
export class ReportsComponent {
  private readonly reportsService = inject(Reports);

  isLoading = signal(false);
  errorMessage = signal('');
  reportHtml = signal('');

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.reportsService.getReportsPage().subscribe({
      next: (html) => {
        this.reportHtml.set(html);
        this.isLoading.set(false);
      },
      error: (error) => {
        this.errorMessage.set(
          error.status === 403
            ? 'You do not have permission to view reports.'
            : 'Unable to load reports.',
        );
        this.isLoading.set(false);
      },
    });
  }
}
