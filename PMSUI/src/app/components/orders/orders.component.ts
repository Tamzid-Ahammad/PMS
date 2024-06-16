import { Component, OnInit } from '@angular/core';
import { Order } from '../../../models/order';
import { OrderService } from '../../../services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];

  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe((orders) => {
      this.orders = orders;
    });
  }
  navigateToCreateOrder(): void {
    this.router.navigate(['/create-order']);
  }
  navigateToUpdateOrder(id: string): void {
    this.router.navigate(['/update-order', id]);
  }
  deleteOrder(id: string): void {
    this.orderService.deleteOrder(id).subscribe(() => {
      this.loadOrders();
    });
  }
}
