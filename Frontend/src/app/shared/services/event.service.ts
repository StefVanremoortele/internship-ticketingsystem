import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { Event, Stock } from '../models';
import { AppConfig } from './../../config/app.config';

@Injectable()
export class EventService {
  private eventsEndpoint = AppConfig.endpoints['events'];
  private headers: HttpHeaders;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  getAllEvents(): Observable<Event[]> {
    return this.http
      .get(this.eventsEndpoint)
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  getEventById(eventId: string): Observable<Event> {
    return this.http
      .get(this.eventsEndpoint + '/' + eventId)
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  getStock(eventId: string): Observable<Stock[]> {
    return this.http
      .get(this.eventsEndpoint + '/' + eventId + '/tickets/stock')
      .map(response => {
        return response as Stock[];
      })
      .catch(error => this.handleError(error));
  }
  createEvent(event: any): Observable<Event> {
    return this.http
      .post(
        this.eventsEndpoint,
        JSON.stringify({
          name: event.name,
          start: event.start,
          end: event.end
        }),
        { headers: this.headers }
      )
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  deleteEventById(id: any): Observable<Array<Event>> {
    const url = `${this.eventsEndpoint}/${id}`;
    return this.http
      .delete(url, { headers: this.headers })
      .map(response => {
        return response;
      })
      .catch(error => this.handleError(error));
  }

  search(eventname: string): Observable<Event[]> {
    return this.http
      .get(`${this.eventsEndpoint}?name=${eventname}`)
      .map((r: Response) => r.json() as any)
      .catch((error: any) => {
        return Observable.throw(error.message || error);
      });
  }

  private handleError(error: any) {
    if (error instanceof Response) {
      return Observable.throw(error.json()['error'] || 'backend server error');
    }
    return Observable.throw(error || 'backend server error');
  }
}
