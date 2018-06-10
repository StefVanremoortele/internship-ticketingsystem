import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { NgModule, Optional, SkipSelf } from "@angular/core";
import { RouterModule } from "@angular/router";

import { throwIfAlreadyLoaded } from "./module-import-guard";

import { ListErrorsComponent } from "./list-errors";
import { UnauthorizedComponent } from "./unauthorized";
import { NotFoundComponent } from "./not-found";
import { MaterialModule } from "../material.module";
import { CountdownTimerComponent } from "./countdown-timer";
import { LoggerService } from "./logger.service";

@NgModule({
  declarations: [
    ListErrorsComponent,
    NotFoundComponent,
    UnauthorizedComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    RouterModule,
    ReactiveFormsModule
  ],
  exports: [
    MaterialModule,
    CommonModule, 
    ListErrorsComponent
  ],
  providers: [LoggerService]
})
export class CoreModule {
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: CoreModule
  ) {
    throwIfAlreadyLoaded(parentModule, "CoreModule");
  }
}
