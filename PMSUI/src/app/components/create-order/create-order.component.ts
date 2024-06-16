import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OrderService } from '../../../services/order.service';
import { Router } from '@angular/router';
import { CreateOrderDto } from '../../../models/create-order-dto';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrl: './create-order.component.css'
})
export class CreateOrderComponent  {
  orderForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private orderService: OrderService,
    private router: Router
  ) {
    this.orderForm = this.fb.group({
      recipientName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      userId: ['', Validators.required],
      orderItems: this.fb.array([this.createOrderItem()])
    });
  }

  createOrderItem(): FormGroup {
    return this.fb.group({
      quantity: [1, Validators.required],
      variantId: ['', Validators.required]
    });
  }

  get orderItems(): FormArray {
    return this.orderForm.get('orderItems') as FormArray;
  }

  addOrderItem(): void {
    this.orderItems.push(this.createOrderItem());
  }

  removeOrderItem(index: number): void {
    this.orderItems.removeAt(index);
  }

  onSubmit(): void {
    if(this.orderForm.valid) {
    const newOrder: CreateOrderDto = this.orderForm.value;
    this.orderService.createOrder(newOrder).subscribe(() => {
      this.router.navigate(['/orders']);
    });
  }
}
}
