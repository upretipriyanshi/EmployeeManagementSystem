import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideStore } from '@ngrx/store';
import { authReducer } from '../store/auth/auth.reducer';
import { taskReducer } from '../store/task/task.reducer';

export const appTestProviders = [
  provideHttpClient(),
  provideHttpClientTesting(),
  provideRouter([]),
  provideStore({ auth: authReducer, tasks: taskReducer }),
];
