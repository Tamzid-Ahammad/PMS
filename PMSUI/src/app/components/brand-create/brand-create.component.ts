import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandService } from '../../../services/brand.service';

@Component({
  selector: 'app-brand-create',
  templateUrl: './brand-create.component.html',
  styleUrl: './brand-create.component.css'
})
export class BrandCreateComponent {
  
  brandForm: FormGroup;

  constructor(private fb: FormBuilder, private brandService: BrandService) {
    this.brandForm = this.fb.group({
      name: ['', Validators.required]
    });
  }

  submit() {
    if (this.brandForm.valid) {
      this.brandService.createBrand(this.brandForm.value).subscribe();
    }
  }
}

