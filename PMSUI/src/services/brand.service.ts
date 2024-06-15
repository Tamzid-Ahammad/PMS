import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { BrandInfo } from '../models/brand-info';

@Injectable({
  providedIn: 'root'
})
export class BrandService {
  private apiUrl = 'https://localhost:44386/api/Brand';

  constructor(private http: HttpClient) { }

  getBrandOptions(): Observable<BrandInfo[]> {
    return this.http.get<BrandInfo[]>(`${this.apiUrl}/GetBrandOptions`);
  }
  getBrandById(id: number): Observable<BrandInfo> {
    return this.http.get<BrandInfo>(`${this.apiUrl}/${id}`);
  }

  createBrand(brand: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Create`, brand);
  }

  createBrands(brands: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/CreateRange`, brands);
  }
}
