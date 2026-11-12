import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { exhaustMap, map, catchError, of, tap } from 'rxjs';

import { AuthService } from '../../services/auth';
import { login, loginSuccess, loginFailure, logout } from './auth.action';

@Injectable()
export class AuthEffects {
  private actions$ = inject(Actions);
  private authService = inject(AuthService);

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(login),

      exhaustMap(({ credentials }) =>
        this.authService.login(credentials).pipe(
          map((response) => loginSuccess({ response })),

          catchError((error) =>
            of(
              loginFailure({
                error:
                  error.status === 401
                    ? 'Invalid email or password.'
                    : 'Login failed. Please try again.',
              }),
            ),
          ),
        ),
      ),
    ),
  );

  persistSession$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(loginSuccess),
        tap(({ response }) => {
          const { token, ...user } = response;
          sessionStorage.setItem('employee-management-auth', JSON.stringify({ user, token }));
        }),
      ),
    { dispatch: false },
  );

  clearSession$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(logout),
        tap(() => sessionStorage.removeItem('employee-management-auth')),
      ),
    { dispatch: false },
  );
}
