import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatListModule } from '@angular/material/list';
import {
  Component,
  Compiler,
  Injectable,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ViewChild,
  ChangeDetectorRef
} from '@angular/core';
import { Location } from '@angular/common';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

import { Subscription } from 'rxjs/Subscription';
import { merge } from 'rxjs/observable/merge';
import { of as observableOf } from 'rxjs/observable/of';
import { catchError } from 'rxjs/operators/catchError';
import { map } from 'rxjs/operators/map';
import { startWith } from 'rxjs/operators/startWith';
import { switchMap } from 'rxjs/operators/switchMap';
import { Observable } from 'rxjs/Observable';

import { User } from 'oidc-client';

import { AuthService, OrderService, fadeInOut } from '../shared/services';
import { HttpError, Ticket, Order, PagedOrder  } from '../shared/models';
import { AppConfig } from '../config/app.config';

@Component({
  selector: 'app-order-history',
  templateUrl: 'order-history.component.html',
  styleUrls: ['order-history.component.scss'],
  animations: [fadeInOut]

})
export class OrderHistoryComponent implements AfterViewInit {
  hasResults: boolean;
  title: String = "Order History";
  displayedColumns = ['orderId', 'orderState', 'dateOpened', 'dateClosed'];
  dataSource = new MatTableDataSource();

  resultsLength = 0;
  isLoadingResults = false;
  isRateLimitReached = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private http: HttpClient,
    private location: Location,
    private authService: AuthService,
    private orderService: OrderService,
    private cd: ChangeDetectorRef
  ) {}

  ngAfterViewInit(): void {
    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 1));

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.getOrderHistory(
            this.sort.active,
            this.sort.direction,
            this.paginator.pageIndex
          );
        }),
        map(data => {
          this.isLoadingResults = false;
          this.isRateLimitReached = false;
          this.resultsLength = data.total_count;
          
          return data.orders;
        }),
        catchError(() => {
          this.isLoadingResults = false;
          // Catch if the API has reached its rate limit. Return empty data.
          this.isRateLimitReached = true;
          return observableOf([]);
        })
      )
      .subscribe((data: Order[]) => {(this.dataSource.data = data); this.resultsLength == 0 ? this.hasResults = false : this.hasResults = true;});
    this.cd.detectChanges();

  }
  getOrderHistory(
    sort: string,
    order: string,
    page: number
  ): Observable<any> {
    return this.orderService.getOrderHistoryFromUserByPage(this.authService.Get().profile.sub, (page));
  }

  goback() {
    this.location.back();
  }

}
