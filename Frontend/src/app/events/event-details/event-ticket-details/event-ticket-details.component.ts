import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  OnDestroy,
  NgZone,
  OnChanges,
  ViewContainerRef
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

import { Observable } from "rxjs/Observable";
import { ErrorObservable } from "rxjs/observable/ErrorObservable";
import {
  HttpClient,
  HttpHeaders,
  HttpResponse,
  HttpErrorResponse
} from "@angular/common/http";
import { Http, Headers, RequestOptions } from "@angular/http";
import { fadeInOut } from "../../../shared/services/animations";
import { AppConfig } from "../../../config/app.config";
import {
  AuthService,
  TicketService,
  Event,
  Ticket,
  Stock,
  SignalRService
} from "../../../shared";
import { HttpError, Order, StockUpdate } from "../../../shared/models";
import { Subscription } from "@aspnet/signalr";
import { ToastsManager, ToastModule } from "ng2-toastr";

@Component({
  // tslint:disable-next-line:component-selector
  selector: "app-event-ticket-details",
  templateUrl: "./event-ticket-details.component.html",
  styleUrls: ['./event-ticket-details.component.scss'],
  animations: [fadeInOut]
})
export class EventTicketDetailsComponent
  implements OnInit, OnChanges, OnDestroy {
  @Input() event: Event;
  @Input() stock: Stock;
  @Output() errorMsg = new EventEmitter<string>();
  maxAmount = 10;
  amount: string;
  @Input() name: string;

  ticketDetails: Stock;
  amountOfAvailableTickets: number;
  stockSubscription: Subscription<any>;
  stockConn: Subscription<any>;
  loading: boolean;

  signalrGroup: string;

  constructor(
    private _ngZone: NgZone,
    private authService: AuthService,
    private ticketService: TicketService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private _signalRService: SignalRService,
    public toastr: ToastsManager,
    private vcr: ViewContainerRef,
    private ngZone: NgZone
  ) {
    this.name = "stef"; // toaster test
    this.toastr.setRootViewContainerRef(vcr);

  }

  ngOnInit() {
    this.signalrGroup = this.event.name + '-' + this.ticketDetails.ticketType;

    this._signalRService.start();
    this._signalRService.connectionEstablished.subscribe(res => {
      if (res === true) {
        this._signalRService.registerToGroup(this.signalrGroup).then(() =>
          this._signalRService.stockChanged.subscribe((data: StockUpdate) => {
            if (data.ticketType === this.ticketDetails.ticketType) {
              this.amountOfAvailableTickets = data.amount;
            }
          })
        );
      }
    });
  }

  ngOnChanges() {
    console.log('eventTicketDetailsComponent onChanges');
    this.ticketDetails = this.stock;
    this.amountOfAvailableTickets = this.ticketDetails.amount;
    this.loading = false;

  }

  ngOnDestroy(): void {
    console.log('eventTicketDetailsComponent onDestroy');
    this._signalRService.stop();
  }

  doToast() {
    this.toastr.custom("Alert", "YES !");
    console.log(this.toastr);
    console.log("Toasty");
  }

  popToast() {}

  reserveTicketByType(ticketType: string, amount: string) {
    this.loading = true;
    this.ticketService
      .processTicketForReservation(ticketType, this.event.eventId, amount)
      .subscribe(
        (res: HttpResponse<any>) => {
          this.loading = false;
          this.doToast();
          // tslint:disable-next-line:no-unused-expression
          // this.availableTickets = this.availableTickets - Number(amount);
        },
        err => this.handleError(err),
        () => (this.loading = false)
      );
  }

  setTicketColor(): string {
    if (this.ticketDetails.ticketType.toLocaleLowerCase() === 'promo') {
      return '#498f37';
    }
    if (this.ticketDetails.ticketType.toLocaleLowerCase() === 'early_bird') {
      return '#88BD25';
    }
    if (this.ticketDetails.ticketType.toLocaleLowerCase() === 'vip') {
      return 'grey';
    }
    if (this.ticketDetails.ticketType.toLocaleLowerCase() === 'group') {
      return '#209C48';
    }
  }

  private handleError(error: HttpErrorResponse) {
    const dataError = new HttpError();
    dataError.statusCode = error.status;
    dataError.message = error.statusText;
    dataError.friendlyMessage = error.error.text;

    this.ngZone.run(() => {
      this.doToast();
  });

    this.errorMsg.emit(dataError.friendlyMessage);
    this.loading = false;
    if (error instanceof Response) {
      return Observable.throw(dataError['error'] || 'backend server error');
    }
    return Observable.throw(dataError || 'backend server error');
  }
}
