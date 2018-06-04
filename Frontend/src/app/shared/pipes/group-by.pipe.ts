import { Pipe, PipeTransform } from "@angular/core";

@Pipe({ name: "groupByType" })
export class CategoryPipe implements PipeTransform {
  transform(value, args: string[]): any {
    const groups = {};
    value.forEach(function(o) {
      const group = o.ticketCategory.type;
      groups[group] = groups[group]
        ? groups[group]
        : { name: group, resources: [] };
      groups[group].resources.push(o);
    });

    return Object.keys(groups).map(function(key) {
      return groups[key];
    });
  }
}

@Pipe({ name: 'available' })
export class AvailableTicketsPipe implements PipeTransform {
  transform(value, args: string[]): any {
    const groups = [];
    value.forEach(function(o) {
      if (o.ticketStatus.toLowerCase() === 'available') {
        groups.push(o);
      }
    });

    return Object.keys(groups).map(function(key) {
      return groups[key];
    });
  }
}
