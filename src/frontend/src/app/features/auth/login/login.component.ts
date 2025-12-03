import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Template-driven form cho nhanh
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzCardComponent } from "ng-zorro-antd/card";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, NzFormModule, NzInputModule, NzButtonModule, NzCardComponent],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  email = '';
  password = '';
  isLoading = false;

  private authService = inject(AuthService);
  private router = inject(Router);
  private message = inject(NzMessageService);

  onSubmit() {
    this.isLoading = true;
    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        this.message.success('Đăng nhập thành công!');
        this.router.navigate(['/']); // Về trang chủ
      },
      error: (err) => {
        this.message.error('Đăng nhập thất bại. Kiểm tra lại email/pass.');
        this.isLoading = false;
      }
    });
  }
}