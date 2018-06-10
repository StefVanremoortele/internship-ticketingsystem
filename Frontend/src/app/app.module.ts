import { ApplicationRef, EventEmitter, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CdkTableModule } from '@angular/cdk/table';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FlexLayoutModule, FlexFillDirective} from '@angular/flex-layout';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';

import { ToastModule, ToastOptions } from 'ng2-toastr';
import './../polyfills';
import { APP_CONFIG, AppConfig } from './config/app.config';

import { AppComponent } from './app.component';
import { AccountModule } from './account';
import { EventsModule } from './events';
import { CartComponent } from './cart';
import { authProviders, routing } from './app-routing.module';
import { CoreModule, CountdownTimerComponent, GlobalErrorHandler, ErrorLoggerService } from './core';
import { HomeComponent } from './home';
import { MyTicketsComponent } from './my-tickets';
import { OrderHistoryComponent } from './order-history';
import { AddAuthorizationHeaderInterceptor } from './core/add-authorization-header-interceptor';
import { HandleHttpErrorInterceptor } from './core/handle-http-error-interceptor';
import { EnsureAcceptHeaderInterceptor } from './core/ensure-accept-header-interceptor';
import { WriteOutJsonInterceptor } from './core/write-out-json-interceptor';
import { SharedModule } from '.';

export class CustomOption extends ToastOptions {
  animate = 'fade';
  newestOnTop = true;
  positionClass = 'toast-top-center';
  enableHTML = true;
  showCloseButton: true;
  dismiss: 'click';
}

@NgModule({
  declarations: [
    AppComponent,
    CartComponent,
    HomeComponent,
    MyTicketsComponent,
    OrderHistoryComponent,
    CountdownTimerComponent
  ],
  imports: [
    CoreModule,
    EventsModule,
    AccountModule,
    SharedModule,
    routing,
    ToastModule.forRoot()
  ],
  providers: [,
    authProviders,
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
    this._appRef.bootstrap(AppComponent);
  }
}
