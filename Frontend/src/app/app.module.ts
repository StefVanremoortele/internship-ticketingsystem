import { ApplicationRef, EventEmitter, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CdkTableModule } from '@angular/cdk/table';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FlexFillDirective } from '@angular/flex-layout';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import './../polyfills';

import { ToastModule, ToastOptions } from 'ng2-toastr';

import { APP_CONFIG, AppConfig } from './config/app.config';

import { AccountComponent } from './account';
import { authProviders, routing } from './app-routing.module';
import { AppComponent } from './app.component';
import { CartComponent } from './cart';
import { CoreModule, CountdownTimerComponent, GlobalErrorHandler, ErrorLoggerService } from './core';
import { EventsModule } from './events';
import { HomeComponent } from './home';
import { MaterialModule } from './material.module';
import { MyTicketsComponent } from './my-tickets';
import { OrderHistoryComponent } from './order-history';
import {
  UserService,
  EventService,
  TicketService,
  AuthService,
  OrderService,
  SharedModule,
  CategoryPipe,
  AvailableTicketsPipe,
  SignalRService,
  DataService,
  HighlightDirective,
  AutofocusDirective
} from './shared';
import { AddAuthorizationHeaderInterceptor } from './core/add-authorization-header-interceptor';
import { HandleHttpErrorInterceptor } from './core/handle-http-error-interceptor';
import { EnsureAcceptHeaderInterceptor } from './core/ensure-accept-header-interceptor';
import { WriteOutJsonInterceptor } from './core/write-out-json-interceptor';

export class CustomOption extends ToastOptions {
  animate = 'fade'; // you can override any options available
  newestOnTop = false;
  positionClass = 'toast-top-right';
  enableHTML = true;
  showCloseButton: true;
}

@NgModule({
  declarations: [
    AccountComponent,
    AppComponent,
    CartComponent,
    HomeComponent,
    MyTicketsComponent,
    OrderHistoryComponent,
    CountdownTimerComponent,
    AutofocusDirective,
    HighlightDirective
  ],
  imports: [
    BrowserModule,
    CoreModule,
    EventsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    MaterialModule,
    routing,
    HttpClientModule,
    FlexLayoutModule,
    ToastModule.forRoot()
  ],
  providers: [
    UserService,
    EventService,
    TicketService,
    AuthService,
    OrderService,
    authProviders,
    SignalRService,
    DataService,
    GlobalErrorHandler,
    ErrorLoggerService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AddAuthorizationHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: EnsureAcceptHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: WriteOutJsonInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HandleHttpErrorInterceptor,
      multi: true,
    },
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    },
    {
      provide: ToastOptions,
      useClass: CustomOption
    },
  ],
  entryComponents: [AppComponent],
    bootstrap: [AppComponent]
})
export class AppModule {
  constructor(private _appRef: ApplicationRef) {}

  ngDoBootstrap() {
    // document.addEventListener('WebComponentsReady', () => {
    this._appRef.bootstrap(AppComponent);
    // });
  }
}
