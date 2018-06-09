import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';

import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { Observable } from 'rxjs/Observable';
import { catchError, map } from 'rxjs/operators';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { Order, PagedOrder } from '../models';
import { HttpError } from '../models/httpError';
import { AppConfig } from '../../config/app.config';
import { AuthService } from './authentication.service';

@Injectable()
export class OrderService {
  private accountEndpoint = AppConfig.endpoints['account'];
  private ordersEndpoint = AppConfig.endpoints['orders'];
  private headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  getCartFromUser(userId: string): Observable<Order> {
    return this.http.get(this.accountEndpoint + '/cart')
      .pipe(
        map(
        (res: Order) => {
            console.log(res);
            return res;
          },
          err => {
            return err;
          }
        )
      );
  }

  getOrderHistoryFromUser(userId: string): Observable<Order> {
    return this.authService
      .AuthGet(this.accountEndpoint + '/orders/history')
      .pipe(
        map(res => {
          return res.body;
        }),
        catchError((err, caught) => {
          return this.handleHttpError(err);
        })
      );
  }

  getOrderHistoryFromUserByPage(userId: string, page: number): Observable<any> {
    return this.authService
      .AuthGet(
      this.ordersEndpoint +
        '/historyByPage?PageNumber=' +
        (page + 1) +
        '&PageSize=' +
        10
      );
  }

  completeOrder(userId: string, orderId: string): Observable<Order> {
    return this.authService
      .AuthGet(
        this.accountEndpoint + '/orders/' + orderId + '/complete'
      )
      .pipe(
        map(
          res => {
            return res.body;
          },
          err => {
            return err;
          }
        )
      );
  }

  cancelOrder(userId: string, orderId: string): Observable<Order> {
    return this.authService
      .AuthGet(
        this.accountEndpoint + '/cart/cancel'
      )
      .pipe(
        map(res => {
          return res.body;
        }),
        catchError((err, caught) => {
          return this.handleHttpError(err);
        })
      );
  }

  cancelTicketFromOrder(userId: string, ticketId: string): Observable<Order> {
    return this.authService
      .AuthGet(
        this.accountEndpoint +
          '/cart/tickets/' +
          ticketId +
          '/cancel')
      .pipe(
        map(res => {
          return res.body;
        }),
        catchError((err, caught) => {
          return this.handleHttpError(err);
        })
      );
  }

  private handleHttpError(error: HttpErrorResponse): Observable<HttpError> {
    const dataError = new HttpError();
    dataError.statusCode = error.status;
    dataError.message = error.statusText;
    dataError.friendlyMessage = error.error;
    return ErrorObservable.create(dataError);
  }
}
