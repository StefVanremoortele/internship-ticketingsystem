import {
  Component,
  Compiler,
  ChangeDetectionStrategy,
  OnInit,
  OnDestroy
} from "@angular/core";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Subscription } from "rxjs";
import { Observable } from "rxjs/Observable";
import { User } from "oidc-client";
import { ApplicationUser } from "./../shared/models/applicationuser.model";
import { AuthService } from "./../shared/services/authentication.service";


@Component({
  selector: "app-account",
  templateUrl: "./account.component.html",
  styleUrls: ["account.component.scss"]
})
export class AccountComponent implements OnInit {
  title: string = "Account";
  userSub: Subscription;
  currentUser: User;

  isAdmin: boolean;
  userRoles: String[];

  accountMenu: String[];

  constructor(private authService: AuthService) {
    this.userSub = this.authService.userLoadededEvent.subscribe(u => {
      this.currentUser = u;
    });
    setTimeout(() => this.authService.getUser());
    setTimeout(() => {
      // load the user
      this.currentUser = this.authService.Get();
      this.userRoles = this.currentUser.profile.role;

      this.isAdmin = this.userRoles.indexOf("administrator") > -1;

      // fill the menu tabs
      this.accountMenu = ["profile"];
      if (this.isAdmin) {
        this.accountMenu.push("users", "roles");
      }
    });
  }

  ngOnInit(): void {

  }

}
