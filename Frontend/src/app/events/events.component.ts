import { ActivatedRoute, Router } from '@angular/router';
import { Component, Compiler, OnInit } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { trigger, style, transition, animate, keyframes, query, stagger } from '@angular/animations';

import { Observable } from 'rxjs/Observable';
import { UserManager, User, Log } from 'oidc-client';

import { AuthService, UserService, EventService } from '../shared/services/';
import { Event, Stock } from '../shared/models';
import { fadeInOut } from '../shared/services/animations';


@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  providers: [],
  styleUrls: ['./events.component.scss'],
  animations: [fadeInOut]
})

export class EventsComponent implements OnInit {
  title: string;
  events$: Observable<Event[]>;

  constructor(private eventService: EventService,
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
  ) {
    this.title = "Events";
  }

  ngOnInit(): void {
    this.events$ = this.eventService.getAllEvents();
  }

  showTickets(event: Event): void {
    const link = ['events', event.eventId];
    this.router.navigate(link);
  }
}
