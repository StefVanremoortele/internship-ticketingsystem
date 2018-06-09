import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MomentModule } from 'ngx-moment';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './../material.module';

import { CategoryPipe, AvailableTicketsPipe, EventDatePipe } from './pipes';
import { AccountService } from './services/account.service';
import { FormsModule } from '@angular/forms';
import { TruncatePipe } from './pipes/truncate.pipe';

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
    FormsModule
  ],
  exports: [CategoryPipe, AvailableTicketsPipe, EventDatePipe, TruncatePipe, FormsModule],
  providers: [
    AccountService
  ]
})
export class SharedModule {}
