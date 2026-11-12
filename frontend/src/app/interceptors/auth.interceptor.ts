import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { switchMap, take } from 'rxjs';

import { selectAuthToken } from '../store/auth/auth.selectors';
import { API_BASE_URL } from '../config/api-base-url';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const store = inject(Store);

  return store.select(selectAuthToken).pipe(
    take(1),

    switchMap((token) => {
      const isBackendRequest = req.url.startsWith(`${API_BASE_URL}/`);

      if (!token || !isBackendRequest) {
        return next(req);
      }

      const authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });

      return next(authReq);
    }),
  );
};
