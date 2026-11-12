import { createAction, props } from '@ngrx/store';
import { LoginRequest, LoginResponse } from '../../models/auth.model';

// Login request
export const login = createAction('[Auth] Login', props<{ credentials: LoginRequest }>());

// Login successful
export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ response: LoginResponse }>(),
);

// Login failed
export const loginFailure = createAction('[Auth] Login Failure', props<{ error: string }>());

// Logout
export const logout = createAction('[Auth] Logout');
