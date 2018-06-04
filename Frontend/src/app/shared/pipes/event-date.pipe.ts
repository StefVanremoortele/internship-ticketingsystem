// ======================================
// Author: Ebenezer Monney
// Email:  info@ebenmonney.com
// Copyright (c) 2017 www.ebenmonney.com
//
// ==> Gun4Hire: contact@ebenmonney.com
// ======================================

import { Pipe, PipeTransform } from '@angular/core';



@Pipe({ name: 'eventDate' })
export class EventDatePipe implements PipeTransform {

    transform(date: any, args?: any): any {
        // TODO: Write custom EventDatePipe logic

        return date;

      }
}
