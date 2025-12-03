import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/identity-service/auth';

  // Dùng Signal (Angular 16+) để quản lý trạng thái user reactive
  currentUser = signal<any | null>(this.getUserFromStorage());

  login(data: any) {
    return this.http.post<any>(`${this.baseUrl}/login`, data).pipe(
      tap((response) => {
        if (response && response.token) {
          // 1. Lưu token vào LocalStorage
          localStorage.setItem('token', response.token);
          // 2. Cập nhật state
          this.currentUser.set({ email: data.email }); 
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUser.set(null);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  private getUserFromStorage() {
    const token = this.getToken();
    return token ? { email: 'User' } : null; // Logic tạm, sau này decode token lấy tên thật
  }
}