import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { distinctUntilChanged, map, catchError, tap } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { ReplaySubject } from 'rxjs/ReplaySubject';

import { ApplicationUser } from '../models/applicationuser.model';
import { AppConfig } from './../../config/app.config';
import { AuthService } from './authentication.service';

@Injectable()
export class UserService {
  private accountEndpoint = AppConfig.endpoints['account'];
  private usersEndpoint = AppConfig.endpoints['users'];

  constructor(private http: HttpClient, private authService: AuthService) { }

  getAll() {
    return this.http.get<ApplicationUser[]>(this.usersEndpoint);
  }

  getAllUserRoles() {
    return this.http.get<any>(this.usersEndpoint + '/roles');
  }

  getById(userId: string): Observable<ApplicationUser> {
    return this.http.get(this.usersEndpoint + '/' + userId)
      .pipe(
        map(
          (res: ApplicationUser) => {
            return res;
          },
          err => {
            return err;
          }
        )
      );
  }

  create(user: ApplicationUser) {
    return this.http.post('/api/users', user);
  }

  
  update(userForUpdate: ApplicationUser) : Observable<any> {
    console.log("Updating in userservice" );
    console.log(userForUpdate );
    return this.http.put(this.accountEndpoint + '/profile/update', userForUpdate).pipe(
      tap(_ => console.log('Updated the user')),
    );;
  }

  delete(id: number) {
    return this.http.delete('/api/users/' + id);
  }

  private handleError(error: any) {
    if (error instanceof Response) {
      return Observable.throw(error.json()['error'] || 'backend server error');
    }
    return Observable.throw(error || 'backend server error');
  }
}
