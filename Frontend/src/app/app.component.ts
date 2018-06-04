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

  loggedInNav = AppConfig.loggedInRoutes;
  loggedOutNav =  AppConfig.loggedOutRoutes;

  constructor(
    private authService: AuthService,
    public toastr: ToastsManager,
    private vcr: ViewContainerRef) {

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
    this.popToast("Logging in...");
    setTimeout(() => this.authService.startSigninMainWindow(), 1500);
  }

  logout() {
    this.popToast("Logging out...");
    setTimeout(() => this.authService.startSignoutMainWindow(), 1500);
  }

  doToast() {
    this.toastr.warning("Is Logged In? " + this.currentUser);
  }

  popToast(msg: string) {
    this.toastr.warning(msg, 'Title', {
      iconClass: 'toast-pink',
      timeOut: 0,
      extendedTimeOut: 0,
      manageClass: 'toastr-custom'
    });
  }
}
