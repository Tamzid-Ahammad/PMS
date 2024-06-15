import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { BrandService } from '../../../services/brand.service';
import { Product } from '../../../models/product';
import { BrandInfo } from '../../../models/brand-info';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrl: './product-update.component.css'
})
export class ProductUpdateComponent implements OnInit {
  updateProductForm!: FormGroup;
  productId!: number;
  brands: BrandInfo[] = [];
  productTypes = ['Mug', 'Jug', 'Cup']; 

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private productService: ProductService,
    private brandService: BrandService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.productId = +id;
      this.initForm();
      this.loadBrands();
      this.loadProduct();
    } else {
      // Handle the error or redirect
      console.error('Invalid product ID');
      this.router.navigate(['/products']);
    }
  }

  initForm() {
    this.updateProductForm = this.fb.group({
      name: ['', Validators.required],
      type: ['', Validators.required],
      brandId: ['', Validators.required],
      variants: this.fb.array([])
    });
  }

  //loadBrands() {
  //  this.brandService.getBrandOptions().subscribe((brands: any) => {
  //    this.brands = brands;
  //  });
  //}
  loadBrands() {
    this.brandService.getBrandOptions().subscribe((brands: BrandInfo[]) => {
      this.brands = brands;
    });
  }
  loadProduct() {
    this.productService.getProductById(this.productId).subscribe((product: Product) => {
      this.updateProductForm.patchValue({
        name: product.name,
        type: product.type,
        brandId: product.brandId
      });
      product.variants.forEach(variant => {
        this.variants.push(this.fb.group({
          id: [variant.id],
          color: [variant.color, Validators.required],
          specification: [variant.specification, Validators.required],
          size: [variant.size, Validators.required],
          price: [variant.price, Validators.required]
        }));
      });
    });
  }

  get variants(): FormArray {
    return this.updateProductForm.get('variants') as FormArray;
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

  onSubmit() {
    if (this.updateProductForm.invalid) {
      return;
    }

    this.productService.updateProduct(this.productId, this.updateProductForm.value)
      .subscribe(() => {
        this.router.navigate(['/products']);
      });
  }

}
