import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AuthService } from './../../shared/services/authentication.service';

@Component({
  selector: 'app-unauthorized',
  templateUrl: 'unauthorized.component.html',
  styleUrls: ['unauthorized.component.scss']
})
export class UnauthorizedComponent implements OnInit {
  constructor(private location: Location, private authService: AuthService) {

  }

  ngOnInit() {
  }

  login() {
    this.authService.startSigninMainWindow();
  }

  goback() {
    this.location.back();
  }
}
