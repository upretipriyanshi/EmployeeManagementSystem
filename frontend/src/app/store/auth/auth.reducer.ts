import { createReducer, on } from '@ngrx/store';
import { initialAuthState } from './auth.state';
import { login, loginSuccess, loginFailure, logout } from './auth.action';

export const authReducer = createReducer(
  initialAuthState,

  // User starts logging in
  on(login, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  // Login succeeds
  on(loginSuccess, (state, { response }) => {
    const { token, ...user } = response;

    return {
      ...state,
      user,
      token,
      loading: false,
      error: null,
      isAuthenticated: true,
    };
  }),

  // Login fails
  on(loginFailure, (state, { error }) => ({
    ...state,
    user: null,
    token: null,
    loading: false,
    error,
    isAuthenticated: false,
  })),

  // User logs out
  on(logout, () => initialAuthState),
);
