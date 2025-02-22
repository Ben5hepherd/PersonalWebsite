import { HttpInterceptorFn } from '@angular/common/http';

export const tokenInterceptor: HttpInterceptorFn = (request, next) => {
  if (typeof window !== 'undefined') {
    const token = localStorage.getItem('bearer-token') || '';
    if (token) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }
  }
  return next(request);
};
