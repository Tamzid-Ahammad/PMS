import { CreateOrderItemDto } from "./create-order-item-dto";

export class CreateOrderDto {
  recipientName!: string;
  email!: string;
  phone!: string;
  address!: string;
  userId!: string;
  orderItems!: CreateOrderItemDto[];
}
