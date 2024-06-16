import { OrderItem } from "./order-item";

export class Order {
  id!: string;
  recipientName!: string;
  email!: string;
  phone!: string;
  address!: string;
  userId!: string;
  orderItems!: OrderItem[];
}
