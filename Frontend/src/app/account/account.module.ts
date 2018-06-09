import { UsersComponent } from "./users/users.component";
import { RolesComponent } from "./roles/roles.component";
import { ProfileComponent } from "./profile/profile.component";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/Forms";
import { AccountComponent } from "./account.component";
import { MaterialModule } from "../material.module";
import { SharedModule } from "../shared";

@NgModule({
  imports: [MaterialModule, CommonModule, SharedModule],
  declarations: [
    AccountComponent,
    ProfileComponent,
    RolesComponent,
    UsersComponent
  ]
})
export class AccountModule {}
