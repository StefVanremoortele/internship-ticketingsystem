import { OnDestroy, OnInit, Injectable, ViewContainerRef } from '@angular/core';
import { ToastsManager, ToastModule } from "ng2-toastr";

@Injectable()
export class ToastService {

  constructor(
    private vcr: ViewContainerRef,
    public toastr: ToastsManager
  ) {
    this.toastr.setRootViewContainerRef(vcr);
  }

  popInfoToast(msg: string, title: string) {
    this.toastr.info(msg, title);
  }
  // this.toastr.info(msg, title, {
  //   timeOut: 0,
  //   extendedTimeOut: 0,
  //   showCloseButton: true
  // })

  popWarningToast(msg: string, title: string) {
    this.toastr.error(msg, title);
  }
 
}
