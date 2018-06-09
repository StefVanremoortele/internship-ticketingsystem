import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/services';
import { ApplicationUser } from '../../shared/models';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  users: ApplicationUser[];

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getAll().subscribe((users: any) => {
    this.users = users;
    
      console.log(this.users);
    })
  }
  showInfo() {
    // not implemented
  }
}
