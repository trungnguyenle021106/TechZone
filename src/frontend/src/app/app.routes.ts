import { Routes } from '@angular/router';
import { CatalogComponent } from './features/catalog/catalog/catalog.component';
export const routes: Routes = [
  { path: '', component: CatalogComponent }, // Trang chủ vào thẳng Catalog
  { path: '**', redirectTo: '', pathMatch: 'full' }
];