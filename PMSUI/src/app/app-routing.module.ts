import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { BrandListComponent } from './components/brand-list/brand-list.component';
import { BrandCreateComponent } from './components/brand-create/brand-create.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';

const routes: Routes = [
  { path: 'products', component: ProductListComponent },
  { path: 'products/create', component: ProductCreateComponent },
  { path: 'brands', component: BrandListComponent },
  { path: 'brands/create', component: BrandCreateComponent },
  { path: 'products/edit/:id', component: ProductUpdateComponent },
  { path: '', redirectTo: '/products', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
