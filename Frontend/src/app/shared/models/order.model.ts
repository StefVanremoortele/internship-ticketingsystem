import { Ticket } from "./";

export class Order {
  constructor(
    public orderId: string,
    public orderState: string,
    public dateOpened: Date,
    public dateClosed: Date,
    public tickets: Ticket[]
  ) {}
}

export class PagedOrder {
  orders: Order[];
  total_count: number;
}
