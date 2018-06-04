
import { Directive, Renderer, ElementRef, OnInit } from '@angular/core';


@Directive({
    selector: '[appAutofocus]'
})
export class AutofocusDirective implements OnInit {
    constructor(public renderer: Renderer, public elementRef: ElementRef) { }

    ngOnInit() {
        setTimeout(() => this.renderer.invokeElementMethod(this.elementRef.nativeElement, 'focus', []), 300);
    }
}
