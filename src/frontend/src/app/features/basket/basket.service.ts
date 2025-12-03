import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Basket, IBasket, IBasketItem } from '../../shared/models/basket';
import { Product } from '../../features/catalog/catalog.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/basket-service';
  
  // Dùng Signal để các component khác (như Header) tự động cập nhật số lượng
  basketSource = signal<IBasket | null>(null);

  // 1. Lấy giỏ hàng từ API (Redis)
  getBasket(id: string) {
    return this.http.get<IBasket>(`${this.baseUrl}?id=${id}`).subscribe({
      next: basket => this.basketSource.set(basket)
    });
  }

  // 2. Cập nhật giỏ hàng lên API
  setBasket(basket: IBasket) {
    return this.http.post<IBasket>(this.baseUrl, basket).subscribe({
      next: response => this.basketSource.set(response),
      error: err => console.log('Lỗi lưu giỏ hàng: ', err)
    });
  }

  // 3. Logic: Thêm sản phẩm vào giỏ
  addItemToBasket(product: Product, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductToBasketItem(product, quantity);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket); // Gửi lên Server
  }

  // --- Các hàm phụ trợ (Helper Methods) ---

  private getCurrentBasket() {
    return this.basketSource();
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id); // Lưu ID giỏ hàng để lần sau vào lại web vẫn còn
    return basket;
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private mapProductToBasketItem(product: Product, quantity: number): IBasketItem {
    return {
      id: parseInt(product.id), // Giả sử ID backend là số, nếu là string thì bỏ parseInt
      productName: product.name,
      price: product.price,
      pictureUrl: '', // Chưa có ảnh thì để rỗng
      quantity,
      brand: 'Product Brand',
      type: 'Product Type'
    };
  }
}