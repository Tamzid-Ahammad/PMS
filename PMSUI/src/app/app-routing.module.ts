import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { BrandListComponent } from './components/brand-list/brand-list.component';
import { BrandCreateComponent } from './components/brand-create/brand-create.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { OrdersComponent } from './components/orders/orders.component';
import { OrderDetailsComponent } from './components/order-details/order-details.component';
import { CreateOrderComponent } from './components/create-order/create-order.component';
import { UpdateOrderComponent } from './components/update-order/update-order.component';

const routes: Routes = [
  { path: 'products', component: ProductListComponent },
  { path: 'products/create', component: ProductCreateComponent },
  { path: 'brands', component: BrandListComponent },
  { path: 'brands/create', component: BrandCreateComponent },
  { path: 'products/edit/:id', component: ProductUpdateComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'orders/:id', component: OrderDetailsComponent },
  { path: 'create-order', component: CreateOrderComponent },
  { path: 'update-order/:id', component: UpdateOrderComponent },
  { path: '', redirectTo: '/products', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
