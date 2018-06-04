
  import { CommonModule } from '@angular/common';
  import { NgModule, Optional, SkipSelf } from '@angular/core';
  import { throwIfAlreadyLoaded } from '../core/module-import-guard';

  import { EventsComponent } from "./events.component";
  import { EventDetailsComponent, EventTicketDetailsComponent } from "./event-details/";
import { SharedModule, MaterialModule, CoreModule } from '..';
import { FormsModule } from '@angular/forms';
import { ToastModule, ToastOptions } from 'ng2-toastr';

export class CustomOption extends ToastOptions {
  animate = 'slideUp'; // you can override any options available
  newestOnTop = false;
  positionClass = 'toast-top-right';
  enableHTML = true;
  showCloseButton: true;
}



@NgModule({
  declarations: [
    EventsComponent,
    EventDetailsComponent,
    EventTicketDetailsComponent
  ],
  imports: [
    MaterialModule,
    SharedModule,
    CommonModule,
    CoreModule,
    FormsModule,
    ToastModule.forRoot()
  ],
  exports: [
  ],
  providers: [
    { provide: ToastOptions, useClass: CustomOption }
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
