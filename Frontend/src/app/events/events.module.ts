
  import { CommonModule } from '@angular/common';
  import { NgModule, Optional, SkipSelf } from '@angular/core';
  import { throwIfAlreadyLoaded } from '../core/module-import-guard';

  import { EventsComponent } from "./events.component";
  import { EventDetailsComponent, EventTicketDetailsComponent } from "./event-details/";
import { SharedModule, MaterialModule, CoreModule } from '..';
import { FormsModule } from '@angular/forms';
import { ToastModule, ToastOptions } from 'ng2-toastr';


@NgModule({
  declarations: [
    EventsComponent,
    EventDetailsComponent,
    EventTicketDetailsComponent
  ],
  imports: [
    MaterialModule,
    SharedModule,
    CoreModule,
    ToastModule.forRoot()
  ],
  exports: [
  ],
  providers: [
  ]
})
export class EventsModule {
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: EventsModule
  ) {
    throwIfAlreadyLoaded(parentModule, 'EventsModule');
  }
}
