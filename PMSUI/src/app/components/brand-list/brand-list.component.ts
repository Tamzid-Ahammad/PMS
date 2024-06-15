import { Component, OnInit } from '@angular/core';
import { BrandInfo } from '../../../models/brand-info';
import { BrandService } from '../../../services/brand.service';

@Component({
  selector: 'app-brand-list',
  templateUrl: './brand-list.component.html',
  styleUrl: './brand-list.component.css'
})
export class BrandListComponent implements OnInit {
  brands: BrandInfo[] = [];

  constructor(private brandService: BrandService) { }

  ngOnInit(): void {
    this.brandService.getBrandOptions().subscribe(data => {
      this.brands = data;
    });
  }
}
