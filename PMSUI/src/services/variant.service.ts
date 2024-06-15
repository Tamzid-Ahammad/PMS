import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class VariantService {
  private apiUrl = 'https://localhost:44386/api/Variant';

  constructor(private http: HttpClient) { }

  deleteVariant(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Delete/${id}`);
  }
}
