import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common'; // Để dùng *ngFor
import { CatalogService, Product } from '../catalog.service';
import { NzCardModule } from 'ng-zorro-antd/card'; // UI Card
import { NzGridModule } from 'ng-zorro-antd/grid'; // UI Grid

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule, NzCardModule, NzGridModule], // Import module UI
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.scss']
})
export class CatalogComponent implements OnInit {
  private catalogService = inject(CatalogService);
  products: Product[] = [];

  ngOnInit(): void {
    this.catalogService.getProducts().subscribe({
      next: (response) => {
        // Lưu ý: Response của bạn có thể là mảng trực tiếp hoặc object { products: [...] }
        // Hãy check console log để map cho đúng. 
        // Giả sử API trả về: { products: [...] } như bài trước ta làm
        this.products = response.products || response; 
      },
      error: (err) => console.error(err)
    });
  }
}