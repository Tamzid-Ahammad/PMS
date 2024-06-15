import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandService } from '../../../services/brand.service';
import { ProductService } from '../../../services/product.service';
import { BrandInfo } from '../../../models/brand-info';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrl: './product-create.component.css'
})
export class ProductCreateComponent implements OnInit {
  productForm: FormGroup;
  brands: BrandInfo[] = [];

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private brandService: BrandService
  ) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      type: ['', Validators.required],
      brandId: ['', Validators.required],
      variants: this.fb.array([
        this.fb.group({
          color: ['', Validators.required],
          specification: ['', Validators.required],
          size: ['', Validators.required],
          price: ['', Validators.required]
        })
      ])
    });
  }

  ngOnInit(): void {
    this.brandService.getBrandOptions().subscribe(data => {
      this.brands = data;
    });
  }

  get variants() {
    return this.productForm.get('variants') as FormArray;
  }

  addVariant() {
    this.variants.push(this.fb.group({
      color: ['', Validators.required],
      specification: ['', Validators.required],
      size: ['', Validators.required],
      price: ['', Validators.required]
    }));
  }

  removeVariant(index: number) {
    this.variants.removeAt(index);
  }

  submit() {
    if (this.productForm.valid) {
      this.productService.createProduct(this.productForm.value).subscribe();
    }
  }
}
