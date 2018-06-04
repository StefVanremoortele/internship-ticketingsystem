import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { Event, Stock } from '../models';
import { AppConfig } from './../../config/app.config';
import { catchError } from 'rxjs/operators';


@Injectable()
export class DataService {
  private eventsEndpoint = AppConfig.endpoints['events'];
  private headers: HttpHeaders;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  test() {
    return this.http
      .get(AppConfig.endpoints['events'] + 'test')
      .pipe(catchError(this.handleError));
  }


  private handleError(error: any) {
    if (error instanceof Response) {
      return Observable.throw(error.json()['error'] || 'backend server error');
    }
    return Observable.throw(error || 'backend server error');
  }
}
