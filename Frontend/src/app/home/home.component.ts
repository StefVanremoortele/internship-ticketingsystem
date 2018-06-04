import {
  Component,
  NgZone,
  OnInit,
  Input,
  ViewContainerRef
} from "@angular/core";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Observable } from "rxjs/Observable";

import { EventService } from "../shared/services/event.service";
import { Event } from "../shared/models/event.model";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"]
})
@Injectable()
export class HomeComponent {
  color: string;
  title: string;

  constructor() {
    this.title = "Home";
  }
}
