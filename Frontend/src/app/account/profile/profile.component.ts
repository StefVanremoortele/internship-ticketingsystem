import { Component, OnInit, Input } from "@angular/core";
import { ApplicationUser, UserService } from "../..";

import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { Observable } from 'rxjs';
import { User } from "oidc-client";
import { map } from "rxjs/operator/map";
@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  isSaving: boolean;
  @Input() currentUser: User;
  @Input() isGeneralEditor = false;
  @Input() isViewOnly: boolean;


  form: FormGroup;
  someError: string;
  user: ApplicationUser;

  isEditMode = false


  constructor(private userService: UserService) {
    this.user = new ApplicationUser("123", "stef@mail.com", "stef", "mylastname");
  }

  ngOnInit(): void {
    if (!this.isGeneralEditor) {
      this.loadCurrentUserData();
    }

    this.form = new FormGroup({
      'id': new FormControl(''),
      'firstname': new FormControl(''),
      'lastname': new FormControl(''),
      'email': new FormControl(''),
      'username': new FormControl('')
    });

    const idCtrl$ = this.form.get('id').valueChanges;
    const firstnameCtrl$ = this.form.get('firstname').valueChanges;
    const lastnameCtrl$ = this.form.get('lastname').valueChanges;
    const emailCtrl$ = this.form.get('email').valueChanges;
    const usernameCtrl$ = this.form.get('username').valueChanges;

    Observable.merge(idCtrl$, usernameCtrl$, firstnameCtrl$, lastnameCtrl$, emailCtrl$)
      .subscribe(() => this.someError = 'ERRORR!!!');

  }


  private loadCurrentUserData() {
    // this.alertService.startLoadingMessage();

    if (this.canViewAllRoles) {   // not yet implemented
      // this.accountService.getUserAndRoles().subscribe(results => this.onCurrentUserDataLoadSuccessful(results[0], results[1]), error => this.onCurrentUserDataLoadFailed(error));
    }
    else {
      this.userService.getById(this.currentUser.profile.sub).subscribe(user => this.onCurrentUserDataLoadSuccessful(user), error => this.onCurrentUserDataLoadFailed(error));
    }
  }


  private onCurrentUserDataLoadSuccessful(user: any) {
    // this.alertService.stopLoadingMessage();
    this.user.userId = user.userId;
    this.user.email = user.email;
    this.user.firstname = user.firstName;
    this.user.lastname = user.lastName;
    this.user.username = user.userName;
    // this.allRoles = roles;
  }



  private onCurrentUserDataLoadFailed(error: any) {
    // this.alertService.stopLoadingMessage();
    // this.alertService.showStickyMessage("Load Error", `Unable to retrieve user data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
    //   MessageSeverity.error, error);;
    
  }



  get canViewAllRoles() {
    // return this.accountService.userHasPermission(Permission.viewRolesPermission);
    return false;
  }


  edit() {
    this.isEditMode = true;
  }
  cancel() {
    this.isEditMode = false;
  }
  save() {
    this.isSaving = true;
    this.userService.update(new ApplicationUser(this.currentUser.profile.sub, this.user.email, this.user.firstname, this.user.lastname, this.user.username)).subscribe(
      result => {
        this.isEditMode = false;
        this.isSaving = false;
        this.user = result;
        this.loadCurrentUserData();
      });
  }
}
