import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

// Định nghĩa kiểu dữ liệu (giống DTO bên C#)
export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  // Angular 16+ dùng inject() thay vì constructor injection truyền thống
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/catalog-service/products';

  getProducts(): Observable<any> { // Chỉnh lại return type chuẩn sau
    return this.http.get<any>(this.baseUrl);
  }
}