import {HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse, HttpEvent} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../app/core/Services/auth.service';
import { AuthStore } from '../app/core/stores/auth.store';

let isRefreshing = false;
let pendingRequests: ((token: string) => void)[] = [];

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);
  const authStore = inject(AuthStore);
  const token = authStore.getAccessToken();

  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && authStore.getRefreshToken()) {
        if (!isRefreshing) {
          isRefreshing = true;

          return authService.refreshToken().pipe(
            switchMap((response) => {
              isRefreshing = false;
              authStore.saveTokens(response.accessToken, response.refreshToken);

              pendingRequests.forEach(cb => cb(response.accessToken));
              pendingRequests = [];

              const retryReq = req.clone({
                setHeaders: { Authorization: `Bearer ${response.accessToken}` }
              });

              return next(retryReq);
            }),
            catchError(err => {
              isRefreshing = false;
              authService.logout();
              return throwError(() => err);
            })
          );
        } else {
          return new Observable<HttpEvent<any>>(observer => {
            pendingRequests.push((newToken: string) => {
              const retryReq = req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` }
              });

              next(retryReq).subscribe({
                next: (event) => observer.next(event),
                error: (err) => observer.error(err),
                complete: () => observer.complete()
              });
            });
          });
        }
      }

      return throwError(() => error);
    })
  );
};
