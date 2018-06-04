import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MomentModule } from 'ngx-moment';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CategoryPipe, AvailableTicketsPipe, EventDatePipe } from './pipes';

@NgModule({
  declarations: [
    CategoryPipe,
    AvailableTicketsPipe,
    EventDatePipe,
  ],
  imports: [
    BrowserAnimationsModule,
    CommonModule,
    HttpClientModule,
    RouterModule,
    MomentModule
  ],
  exports: [CategoryPipe, AvailableTicketsPipe, EventDatePipe]
})
export class SharedModule {}
