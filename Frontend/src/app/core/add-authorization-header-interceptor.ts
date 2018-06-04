import { Injectable } from '@angular/core';
import { HttpRequest, HttpInterceptor, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { AuthService } from '../shared/services';

@Injectable()
export class AddAuthorizationHeaderInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler):
         Observable<HttpEvent<any>> {
        // add the access token as bearer token
        if (this.authService.currentUser) {
          request = request.clone(
              { setHeaders: { Authorization: this.authService.currentUser.token_type
                  + ' ' + this.authService.currentUser.access_token } });
        }
        return next.handle(request);
    }
}
