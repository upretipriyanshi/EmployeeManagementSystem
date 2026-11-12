import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AsyncPipe } from '@angular/common';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { login } from '../../store/auth/auth.action';
import {
  selectAuthLoading,
  selectAuthError,
  selectIsAuthenticated,
} from '../../store/auth/auth.selectors';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, AsyncPipe, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private fb = inject(FormBuilder);
  private store = inject(Store);
  private router = inject(Router);

  loading$ = this.store.select(selectAuthLoading);
  error$ = this.store.select(selectAuthError);
  isAuthenticated$ = this.store.select(selectIsAuthenticated);

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  constructor() {
    this.isAuthenticated$.pipe(takeUntilDestroyed()).subscribe((isAuthenticated) => {
      if (isAuthenticated) {
        this.router.navigate(['/dashboard']);
      }
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.store.dispatch(login({ credentials: this.loginForm.getRawValue() }));
  }
}
