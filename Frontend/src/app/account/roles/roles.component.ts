import { Component, OnInit } from '@angular/core';
import { UserService } from '../..';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {

  roles: any;
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getAllUserRoles().subscribe((roles: any) =>
      this.roles = roles
    )
  }

}
