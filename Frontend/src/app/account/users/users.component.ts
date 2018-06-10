import { Component, OnInit, OnChanges, SimpleChanges, AfterViewChecked, AfterContentChecked } from '@angular/core';
import { UserService } from '../../shared/services';
import { ApplicationUser } from '../../shared/models';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnChanges {
  users: ApplicationUser[];
  
  constructor(private userService: UserService) { }
  
  ngOnInit() {
    this.loadUsers();
  }
  
  loadUsers() {
    this.userService.getAll().subscribe((users: any) => {
      this.users = users;
    })
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    console.log('on Changes being called');
  }
  
  showInfo() {
    // not implemented
  }
}
