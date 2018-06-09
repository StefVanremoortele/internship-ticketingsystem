import { Component, Compiler, OnInit, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { User } from 'oidc-client';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { AppConfig } from '../config/app.config';
import { AuthService, TicketService } from './../shared';
import { Ticket } from './../shared';
import { map } from 'rxjs/operators/map';

@Component({
  selector: 'app-my-tickets',
  templateUrl: './my-tickets.component.html',
  styleUrls: ['./my-tickets.component.scss']
})
export class MyTicketsComponent implements OnInit {
  boughtTickets$: Observable<Ticket[]>;
  boughtTickets: Ticket[];
  title: String = "My Tickets";

  constructor(
    private location: Location,
    private authService: AuthService,
    private ticketService: TicketService

  ) {}

  ngOnInit(): void {
    this.boughtTickets$ = this.ticketService.getBoughtTicketsFromUser(this.authService.Get().profile.sub)
    .pipe(
      map( (res: Ticket[]) => {
        this.boughtTickets = res;
        return res;
      })
    );
  }

  goback() {
    this.location.back();
  }
}



