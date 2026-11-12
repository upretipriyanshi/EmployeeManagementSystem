import { createFeatureSelector, createSelector } from '@ngrx/store';

import { AuthState } from './auth.state';

// Select the complete Auth state
export const selectAuthState = createFeatureSelector<AuthState>('auth');

// Select logged-in user
export const selectAuthUser = createSelector(selectAuthState, (state) => state.user);

// Select JWT token
export const selectAuthToken = createSelector(selectAuthState, (state) => state.token);

// Select login loading status
export const selectAuthLoading = createSelector(selectAuthState, (state) => state.loading);

// Select login error
export const selectAuthError = createSelector(selectAuthState, (state) => state.error);

// Select authentication status
export const selectIsAuthenticated = createSelector(
  selectAuthState,
  (state) => state.isAuthenticated,
);

export const selectUserRole = createSelector(selectAuthUser, (user) => user?.role ?? null);

export const selectAuthUserId = createSelector(selectAuthUser, (user) => user?.userId ?? null);
