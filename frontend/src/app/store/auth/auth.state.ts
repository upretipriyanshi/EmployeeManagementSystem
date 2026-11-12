import { LoginResponse } from '../../models/auth.model';

export interface AuthState {
  user: Omit<LoginResponse, 'token'> | null;
  token: string | null;
  loading: boolean;
  error: string | null;
  isAuthenticated: boolean;
}

function restoreAuthState(): Pick<AuthState, 'user' | 'token' | 'isAuthenticated'> {
  if (typeof sessionStorage === 'undefined') {
    return { user: null, token: null, isAuthenticated: false };
  }

  try {
    const stored = JSON.parse(sessionStorage.getItem('employee-management-auth') ?? 'null') as {
      user?: AuthState['user'];
      token?: string;
    } | null;

    if (stored?.user && stored.token && typeof stored.user.userId === 'number') {
      return { user: stored.user, token: stored.token, isAuthenticated: true };
    }
  } catch {
    sessionStorage.removeItem('employee-management-auth');
  }

  return { user: null, token: null, isAuthenticated: false };
}

export const initialAuthState: AuthState = {
  ...restoreAuthState(),
  loading: false,
  error: null,
};
