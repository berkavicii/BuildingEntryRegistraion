import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    try {
      const token = this.auth.getToken();

      const shouldAttach = !!token && req.url.startsWith(environment.apiBaseUrl);

      const clonedReq = shouldAttach
        ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
        : req;

      return next.handle(clonedReq).pipe(
        tap(event => {
          if (event instanceof HttpResponse) {
            const headerToken = event.headers.get('token') || event.headers.get('authorization') || event.headers.get('Authorization');
            if (headerToken) {
              const cleaned = headerToken.startsWith('Bearer ') ? headerToken.substring(7) : headerToken;
              this.auth.setToken(cleaned);
            }
          }
        })
      );
    } catch (e) {
      return next.handle(req);
    }
  }
}
