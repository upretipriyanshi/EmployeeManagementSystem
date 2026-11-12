import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-setup-admin',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './setup-admin.html',
  styleUrl: './setup-admin.css',
})
export class SetupAdmin {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  submitting = signal(false);
  errorMessage = signal('');

  form = this.formBuilder.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(50)]],
    lastName: ['', [Validators.required, Validators.maxLength(50)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],
    password: ['', [Validators.required, Validators.minLength(12)]],
  });

  submit(): void {
    if (this.form.invalid || this.submitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.errorMessage.set('');
    this.authService.setupInitialAdmin(this.form.getRawValue()).subscribe({
      next: () => this.router.navigate(['/login']),
      error: (error) => {
        this.errorMessage.set(
          error.status === 409
            ? 'An account already exists or administrator setup is unavailable.'
            : 'Unable to create the administrator. Check the API connection and try again.',
        );
        this.submitting.set(false);
      },
    });
  }
}
