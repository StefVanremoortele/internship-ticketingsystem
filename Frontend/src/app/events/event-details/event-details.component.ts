import { Component, Compiler, OnInit, Injectable, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';
import { ActivatedRoute, Params } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { User } from 'oidc-client';

import { AuthService, EventService, TicketService } from '../../shared';
import { Event, Ticket, Stock } from '../../shared/models';
import { CategoryPipe, AvailableTicketsPipe } from '../../shared/pipes';

import { fadeInOut, enterLeave } from '../../shared/services/animations';
import { AppConfig } from './../../config/app.config';

@Component({
  selector: 'app-event-details',
templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss'],
  animations: [fadeInOut, enterLeave]
})
export class EventDetailsComponent implements OnInit {
  loading: Boolean;
  userSub: Subscription;
  currentUser: User;

  event$: Observable<Event>;
  tickets$: Observable<Ticket[]>;
  stock: Stock[];

  errorMsg: string;
  eventId: string;
  reservedTicket: Ticket;
  processing: boolean;

  constructor(
    private location: Location,
    private authService: AuthService,
    private eventService: EventService,
    private ticketService: TicketService,
    private activatedRoute: ActivatedRoute
  ) {
    // ticket reservation indicator
    this.processing = false;
    // get the current user
    this.userSub = this.authService.userLoadededEvent.subscribe(
      u => (this.currentUser = u)
    );
    // // redirect to authority if user isn't logged in
    // this.authService.isLoggedInObs().subscribe(flag => {
    //   if (!flag) {
    //     this.authService.startSigninMainWindow();
    //   }
    // });
  }

  ngOnInit(): void {
    this.loading = true;
    this.activatedRoute.params.subscribe((params: any) => {
      this.eventId = params['id'];
      this.event$ = this.eventService.getEventById(this.eventId);
      this.eventService.getStock(this.eventId).subscribe(
        stock => {
          this.stock = stock;
          this.loading = false;
        }
      );
    });
  }

  setErrorMsg(msg: string) {
    console.log("SETTING");
    this.errorMsg = msg;
  }

  goback() {
    this.location.back();
  }

  private handleError(error: any) {
    if (error instanceof Response) {
      return Observable.throw(error.json()['error'] || 'backend server error');
    }
    return Observable.throw(error || 'backend server error');
  }

  getTicketColor(ticketType: string): string {
    if (ticketType.toLocaleLowerCase() === 'promo') { return '#498f37'; }
    if (ticketType.toLocaleLowerCase() === 'early_bird') {
      return '#88BD25';
    }
    if (ticketType.toLocaleLowerCase() === 'vip') { return 'grey'; }
    if (ticketType.toLocaleLowerCase() === 'group') { return '#209C48'; }
  }


}
