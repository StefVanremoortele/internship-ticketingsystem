import { authProviders } from './../../app-routing.module';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { ReplaySubject } from 'rxjs/ReplaySubject';
import { distinctUntilChanged, map } from 'rxjs/operators';

import { AuthService } from '../../shared/services/authentication.service';
import { Ticket } from '../models';
import { AppConfig } from './../../config/app.config';
import { HttpError } from '..';

@Injectable()
export class TicketService {
  private eventsEndpoint = AppConfig.endpoints['events'];
  private ticketsEndpoint = AppConfig.endpoints['tickets'];
  private usersEndpoint = AppConfig.endpoints['users'];
  private accountEndpoint = AppConfig.endpoints['account'];
  private headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  getAllTickets(): Observable<Ticket[]> {
    return this.http
      .get(this.ticketsEndpoint)
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  getTicketsFromEvent(eventId: string): Observable<Ticket[]> {
    return this.http
      .get(this.eventsEndpoint + '/' + eventId + '/tickets')
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  getBoughtTicketsFromUser(userId: string): Observable<Ticket[]> {
    return this.http.get(
        this.accountEndpoint + '/tickets',
      )
      .pipe(
        map(
          (response: Ticket[]) => {
            return response;
          })
      );
  }

  getTicketById(ticketId: string): Observable<Ticket> {
    return this.authService
      .AuthGet(this.ticketsEndpoint + '/' + ticketId)
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  createTicket(ticket: any): Observable<Ticket> {
    return this.authService
      .AuthPost(
        this.ticketsEndpoint,
        JSON.stringify({
          id: ticket.id,
          userId: ticket.userId,
          orderId: ticket.orderId
        })
      )
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  deleteTicketById(id: any): Observable<Array<Ticket>> {
    const url = `${this.ticketsEndpoint}/${id}`;
    return this.authService
      .AuthDelete(url)
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  processTicketForReservation(
    type: string,
    eventId: string,
    amount: string
  ): Observable<HttpResponse<any> | HttpError> {
    return this.authService.AuthGet(
      this.eventsEndpoint +
        '/' +
        eventId +
        '/tickets/reserve?ticketType=' +
        type +
        '&amount=' +
        amount
    );
  }

  private handleError(error: any) {
    if (error instanceof Response) {
      return Observable.throw(error.json()['error'] || 'backend server error');
    }
    return Observable.throw(error || 'backend server error');
  }
}
