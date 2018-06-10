import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { NgModule, EventEmitter } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormGroup, FormBuilder } from '@angular/forms';

import { AppConfig } from './config/app.config';
import { User } from 'oidc-client';

import { UserService, AuthService, EventService } from './shared/services/';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  userSub: Subscription;
  currentUser: User;

  loggedInNavMenuItems = AppConfig.loggedInRoutes;
  loggedOutNavMenuItems = AppConfig.loggedOutRoutes;

  constructor(
    private authService: AuthService,
    private toastr: ToastsManager,
    private vcr: ViewContainerRef
  ) {
    this.toastr.setRootViewContainerRef(vcr);
  }

  ngOnInit() {
    // setTimeout(() => this.authService.getUser());
    setTimeout(() => (this.currentUser = this.authService.Get()));

    this.currentUser = this.authService.Get();
  }

  isLoggedIn(): boolean {
    return this.authService.loggedIn;
  }

  login() {
    this.toastr.info("Logging in...", "Authentication alert");
    setTimeout(() => this.authService.startSigninMainWindow(), 1500);
  }

  logout() {
    this.toastr.info("Logging out...", "Authentication alert");
    setTimeout(() => this.authService.startSignoutMainWindow(), 1500);
  }
  
  popWarningToast() {
    const msg = "test1";
    const title = "title";
    this.toastr.warning(msg, 'Title', {
      iconClass: 'toast-pink',
      timeOut: 0,
      extendedTimeOut: 0,
      manageClass: 'toastr-custom'
    });
  }

}
