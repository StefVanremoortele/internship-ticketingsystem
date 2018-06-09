import { ActivatedRoute, Router } from "@angular/router";
import {
  Component,
  Compiler,
  Injectable,
  OnInit,
  OnDestroy,
  ViewContainerRef,
  Input
} from "@angular/core";
import {
  HttpClient,
  HttpResponse,
  HttpErrorResponse
} from "@angular/common/http";

import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs";
import { User } from "oidc-client";
import { merge } from "rxjs/observable/merge";
import { of as observableOf } from "rxjs/observable/of";
import { catchError } from "rxjs/operators/catchError";
import { map } from "rxjs/operators/map";
import { startWith } from "rxjs/operators/startWith";
import { switchMap } from "rxjs/operators/switchMap";


import { ToastsManager, ToastModule } from "ng2-toastr";

import { ErrorObservable } from "rxjs/observable/ErrorObservable";
import { tap } from "rxjs/operators";

import { HttpError } from "../shared/models/httpError";
import { fadeInOut } from "../shared/services/animations";
import { AppConfig } from "../config/app.config";
import { CountdownTimerComponent } from "../core/countdown-timer";
import {
  AuthService,
  OrderService,
  UserService,
  CategoryPipe,
  Order
} from "./../shared";


@Component({
  selector: "app-cart",
  templateUrl: "./cart.component.html",
  styleUrls: ["./cart.component.scss"],
  animations: [fadeInOut]
})
export class CartComponent implements OnInit {
  @Input() name: string;
  title: string;
  userSub: Subscription;
  currentUser: User;
  cart$: Observable<Order | HttpError>;
  cart: Order;
  orderId: string;
  errorMsg: string;
  amountOfItems: number;

  expireDate: Date;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private orderService: OrderService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public toastr: ToastsManager,
    private vcr: ViewContainerRef
  ) {
    this.name = "stef"; // toaster test
    this.title = "Cart";
    this.currentUser = this.authService.Get();
    this.toastr.setRootViewContainerRef(vcr);
  }

  ngOnInit() {
    this.loadCart();
  }


  doToast() {
    this.toastr.custom("Alert", "custom !");
    this.toastr.success("Alert", "success !");
    this.toastr.info("Alert", "info !");
    this.toastr.warning("Alert", "warning !");
    this.toastr.error("Alert", "error !");
  }

  popToast() {}

  loadCart() {
    this.cart$ = this.orderService
      .getCartFromUser(this.currentUser.profile.sub)
      .pipe(
        map((res: Order) => {          
          this.amountOfItems = res.tickets.length;
          this.orderId = res.orderId;
          this.cart = res;
          // this.expireDate = new Date(res.dateOpened.setMinutes(res.dateOpened.getMinutes() + 15));
          // this.expireDate.setHours(this.expireDate.getHours() + 2); // TODO: fix general UTC times
          return res;
        })
      );
  }

  cancelCart() {
    this.cart$ = this.orderService
      .cancelOrder(this.currentUser.profile.sub, this.orderId)
      .pipe(
        map(result => {
          if (!result) {
          } else {
            return result;
          }
        }),
        catchError((err, caught) => {
          return this.handleCartError(err);
        })
      )
      .do(() => this.loadCart());
  }

  complete() {
    this.orderService
      .completeOrder(this.currentUser.profile.sub, this.orderId)
      .subscribe(
        () => this.loadCart(),
        catchError(err => {
          return this.handleCartError(err);
        })
      );
  }

  cancelTicket(ticketId: string) {
    this.orderService
      .cancelTicketFromOrder(this.currentUser.profile.sub, ticketId)
      .subscribe(() => this.loadCart());
  }

  continueShopping() {
    this.router.navigate(["events"]);
  }

  goToEvent(eventId: string) {
    this.router.navigate(["events/" + eventId]);
  }

  private handleCartError(error: HttpErrorResponse): Observable<HttpError> {
    const dataError = new HttpError();
    dataError.statusCode = error.status;
    dataError.message = error.statusText;

    if (error.error.orderState.toLocaleLowerCase() === "complete") {
      // tslint:disable-next-line:max-line-length
      this.errorMsg =
        "Your last order #" +
        error.error.orderId +
        " was completed on " +
        new Date(error.error.dateClosed).toDateString() +
        " at " +
        new Date(error.error.dateClosed).toLocaleTimeString() +
        ".";
    }
    if (error.error.orderState.toLocaleLowerCase() === "canceled") {
      // tslint:disable-next-line:max-line-length
      this.errorMsg =
        "Your've canceled your last order #" +
        error.error.orderId +
        " on " +
        new Date(error.error.dateClosed).toDateString() +
        " at " +
        new Date(error.error.dateClosed).toLocaleTimeString() +
        ".";
    }
    if (dataError.statusCode === 404) {
      this.errorMsg = "Please order some tickets first";
      if (error.error != null) {
        dataError.friendlyMessage = error.error;
        if (error.error.orderState.toLocaleLowerCase() === "expired") {
          // tslint:disable-next-line:max-line-length
          this.errorMsg =
            "Your last order (#" +
            error.error.orderId +
            ") expired on " +
            new Date(error.error.dateClosed).toDateString() +
            " at " +
            new Date(error.error.dateClosed).toLocaleTimeString() +
            ".";
        }

      }
    }

    return ErrorObservable.create(dataError);
  }

  calculateSubTotal() {
    let subTotal = 0;
    if (this.cart) {
      this.cart.tickets.forEach(ticket => {
        subTotal += Number(ticket.ticketCategory.price);
      });
    }
    return subTotal;
  }

  calculateBtw() {
    return this.calculateSubTotal() / 100 * 21;
  }

  calculateTotal() {
    return this.calculateSubTotal() + this.calculateBtw();
  }

  getExpireTime() {

    // console.log("expireDate: " + this.expireDate);
    // console.log("The time is now " + new Date());
    return this.expireDate;
  }


  getTicketColor(ticketType: string): string {
    if (ticketType.toLocaleLowerCase() === "promo") {
      return "#498f37";
    }
    if (ticketType.toLocaleLowerCase() === "early_bird") {
      return "#88BD25";
    }
    if (ticketType.toLocaleLowerCase() === "vip") {
      return "grey";
    }
    if (ticketType.toLocaleLowerCase() === "group") {
      return "#209C48";
    }
  }
}
