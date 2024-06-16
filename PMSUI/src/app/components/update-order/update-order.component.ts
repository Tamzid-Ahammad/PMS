import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OrderService } from '../../../services/order.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-update-order',
  templateUrl: './update-order.component.html',
  styleUrl: './update-order.component.css'
})
export class UpdateOrderComponent implements OnInit {
  orderForm: FormGroup;
  orderId: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService
  ) {
    this.orderForm = this.fb.group({
      recipientName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      orderItems: this.fb.array([])
    });
    this.orderId = this.route.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder(): void {
    this.orderService.getOrder(this.orderId).subscribe(order => {
      this.orderForm.patchValue({
        recipientName: order.recipientName,
        email: order.email,
        phone: order.phone,
        address: order.address,
      });

      order.orderItems.forEach(item => {
        this.addOrderItem(item.quantity, item.variantId);
      });
    });
  }

  get orderItems(): FormArray {
    return this.orderForm.get('orderItems') as FormArray;
  }

  addOrderItem(quantity: number = 0, variantId: number = 0): void {
    this.orderItems.push(this.fb.group({
      quantity: [quantity, Validators.required],
      variantId: [variantId, Validators.required]
    }));
  }

  removeOrderItem(index: number): void {
    this.orderItems.removeAt(index);
  }

  onSubmit(): void {
    if (this.orderForm.valid) {
      this.orderService.updateOrder(this.orderId, this.orderForm.value).subscribe(() => {
        this.router.navigate(['/orders']);
      });
    }
  }
}
