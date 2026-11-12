import { Component } from '@angular/core';

import { inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { AsyncPipe } from '@angular/common';
import { selectAuthUser } from '../../store/auth/auth.selectors';
import { Router } from '@angular/router';
import { logout } from '../../store/auth/auth.action';

@Component({
  selector: 'app-navbar',
  imports: [AsyncPipe],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  private store = inject(Store);
  private router = inject(Router);

  user$ = this.store.select(selectAuthUser);

  onLogout(): void {
    this.store.dispatch(logout());
    this.router.navigate(['/login']);
  }
}
