import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MomentModule } from 'ngx-moment';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MaterialModule } from './../material.module';
import { CategoryPipe, AvailableTicketsPipe, EventDatePipe, TruncatePipe } from './pipes';
import {  } from './services/account.service';
import { FlexLayoutModule } from '@angular/flex-layout';
import { OrderService, EventService, TicketService, AuthService, UserService, SignalRService, AccountService } from './services';

@NgModule({
  declarations: [
    CategoryPipe,
    AvailableTicketsPipe,
    TruncatePipe,
    EventDatePipe
  ],
  imports: [
    BrowserAnimationsModule,
    CommonModule,
    HttpClientModule,
    RouterModule,
    MomentModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    FormsModule
  ],
  exports: [CategoryPipe, AvailableTicketsPipe, EventDatePipe, TruncatePipe, FormsModule],
  providers: [
    AccountService,
    UserService,
    EventService,
    TicketService,
    AuthService,
    OrderService,
    SignalRService
  ]
})
export class SharedModule {}
