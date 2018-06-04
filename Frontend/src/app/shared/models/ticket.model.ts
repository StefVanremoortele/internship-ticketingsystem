
import { TicketCategory } from "./ticketCategory.model";

export class Ticket {
  constructor(
    public eventName: string,
    public ticketId: string,
    public orderId: string,
    public eventId: string,
    public ticketStatus: string,
    public ticketCategory: TicketCategory) {
    }
}
