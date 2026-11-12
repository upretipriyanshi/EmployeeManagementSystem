import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { logout } from '../../store/auth/auth.action';

import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  private store = inject(Store);
  private router = inject(Router);

  onLogout(): void {
    this.store.dispatch(logout());
    this.router.navigate(['/login']);
  }
}
