import { Component, Compiler, OnInit, OnDestroy } from "@angular/core";
import { AuthService } from "./../shared/services/authentication.service";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "oidc-client";
import { Subscription } from "rxjs";

@Component({
  selector: "app-account",
  templateUrl: "./account.component.html",
  styleUrls: ['account.component.scss']
})
export class AccountComponent {
  title: string;
  userSub: Subscription;
  currentUser: User;

  constructor(private authService: AuthService) {
    this.title = "Account";
    // this.userSub = this.authService.userLoadededEvent.subscribe(
    //   u => {
    //     this.currentUser = u;
    //   }
    // );
    // setTimeout(() => this.authService.getUser());
    // setTimeout(() => (this.currentUser = this.authService.Get()));

    this.currentUser = this.authService.Get();
  }
}
