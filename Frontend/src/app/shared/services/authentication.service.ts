import { Injectable, EventEmitter } from "@angular/core";
import { Http, Headers, RequestOptions } from "@angular/http";
import {
  HttpClient,
  HttpHeaders,
  HttpResponse,
  HttpErrorResponse
} from "@angular/common/http";

import { ErrorObservable } from "rxjs/observable/ErrorObservable";

// tslint:disable-next-line:import-blacklist
import { Observable } from "rxjs/Rx";
import { Subject } from "rxjs/Subject";
import { catchError } from "rxjs/operators";
import { UserManager, User, Log } from "oidc-client";

import { Order, HttpError } from "./../models";
import { AppConfig } from "../../config/app.config";
import { LoggerService } from "../../core/logger.service";

const settings: any = {
  authority: AppConfig.constants.authorityUrl,
  client_id: "AngularClient",
  redirect_uri: AppConfig.constants.angularClientUrl + "/signin-callback.html",
  post_logout_redirect_uri: AppConfig.constants.angularClientUrl,

  response_type: "token id_token",
  scope: "openid profile roles Ticketingsystem_API",

  silent_redirect_uri:
    AppConfig.constants.angularClientUrl + "/silent-renew.html",
  automaticSilentRenew: true,
  accessTokenExpiringNotificationTime: 4,
  // silentRequestTimeout:10000,
  filterProtocolClaims: true,
  loadUserInfo: true
};

Log.logger = console;
Log.level = Log.DEBUG;

@Injectable()
export class AuthService {
  mgr: UserManager = new UserManager(settings);
  userLoadededEvent: EventEmitter<User> = new EventEmitter<User>();
  currentUser: User;
  loggedIn = false;

  private _loginStatus = new Subject<boolean>();
  private authHeaders: HttpHeaders;

  constructor(
    private httpClient: HttpClient,
    private logService: LoggerService
  ) {
    this.mgr
      .getUser()
      .then(user => {
        if (user) {
          this.loggedIn = true;
          this.currentUser = user;
          this.userLoadededEvent.emit(user);
        } else {
          this.loggedIn = false;
        }
      })
      .catch(err => {
        this.loggedIn = false;
      });

    this.mgr.events.addUserLoaded(user => {
      this.currentUser = user;
      LoggerService.log("authService addUserLoaded");
    });

    this.mgr.events.addUserUnloaded(e => {
      LoggerService.log("user unloaded");
      this.loggedIn = false;
    });
  }

  getLoginStatusEvent(): Observable<boolean> {
    return this._loginStatus.asObservable();
  }

  isLoggedInObs(): Observable<boolean> {
    return Observable.fromPromise(this.mgr.getUser()).map<User, boolean>(
      user => {
        return user ? true : false;
      }
    );
  }

  clearState() {
    this.mgr
      .clearStaleState()
      .then(function() {
        LoggerService.log("authService addUserLoaded");
      })
      .catch(function(e) {
        LoggerService.error("authService addUserLoaded");
      });
  }

  getUser() {
    this.mgr
      .getUser()
      .then(user => {
        this.currentUser = user;
        LoggerService.log("got user");
        this.userLoadededEvent.emit(user);
      })
      .catch(function(err) {
        LoggerService.error(err);
      });
  }

  Get() {
    return this.currentUser;
  }

  getUserId() {
    return this.currentUser.profile.sub;
  }

  removeUser() {
    this.mgr
      .removeUser()
      .then(() => {
        this.userLoadededEvent.emit(null);
        LoggerService.log("user removed");
      })
      .catch(function(err) {
        LoggerService.error(err);
      });
  }

  startSigninMainWindow() {
    this.mgr
      .signinRedirect({ data: "some data" })
      .then(function() {
        LoggerService.log('signinRedirect done');
      })
      .catch(function(err) {
        console.log(err);
      });
  }
  endSigninMainWindow() {
    this.mgr
      .signinRedirectCallback()
      .then(function(user) {
        this.LoggerService.log("signed in", user);
      })
      .catch(function(err) {
        this.LoggerService.error(err);
      });
  }

  startSignoutMainWindow() {
    this.mgr
      .signoutRedirect({ id_token_hint: this.currentUser.id_token })
      .then(function(resp) {
        this.LoggerService.log("signed out", resp);
        setTimeout(5000, () => {
          this.LoggerService.log("testing to see if fired...");
        });
      })
      .catch(function(err) {
        this.LoggerService.error(err);
      });
  }

  endSignoutMainWindow() {
    this.mgr
      .signoutRedirectCallback()
      .then(function(resp) {
        this.LoggerService.log("Logged in");
      })
      .catch(function(err) {
        this.LoggerService.error(err);
      });
  }
  /**
   * @param options if options are not supplied the default content type is application/json
   */
  AuthGet(
    url: string,
    withHeaders?: boolean,
    options?: RequestOptions
  ): Observable<HttpResponse<any>> {
    this._setAuthHeaders(this.currentUser);
    if (withHeaders) {
      return this.httpClient.get<any>(url, {
        headers: this.authHeaders,
        observe: "response"
      });
    } else {
      return this.httpClient
        .get<any>(url, { headers: this.authHeaders })
        .pipe(catchError(err => this.handleHttpError(err)));
    }
  }

  AuthPut(
    url: string,
    data: any,
    options?: RequestOptions
  ): Observable<Response> {
    const body = JSON.stringify(data);
    this._setAuthHeaders(this.currentUser);
    return this.httpClient.put<any>(url, { headers: this.authHeaders });
  }

  AuthDelete(url: string, options?: RequestOptions): Observable<Response> {
    this._setAuthHeaders(this.currentUser);
    return this.httpClient.delete<any>(url, { headers: this.authHeaders });
  }

  AuthPost(
    url: string,
    data: any,
    options?: RequestOptions
  ): Observable<Response> {
    const body = JSON.stringify(data);
    this._setAuthHeaders(this.currentUser);
    return this.httpClient
      .post<any>(url, body)
      .pipe(catchError(err => this.handleHttpError(err)));
  }

  private _setAuthHeaders(user: any): void {
    const headers = new HttpHeaders({
      Authorization: "Bearer " + this.currentUser.access_token,
      "Content-Type": "application/json",
      Accept: `application/json, text/plain, */*`
    });
    this.authHeaders = headers;
  }

  private handleHttpError(error: HttpErrorResponse): Observable<HttpError> {
    const dataError = new HttpError();
    LoggerService.error('Handling error' + error);
    dataError.statusCode = error.status;
    dataError.message = error.statusText;
    dataError.friendlyMessage = error.error;
    return ErrorObservable.create(dataError);
  }
}
