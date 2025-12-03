import { Routes } from '@angular/router';
import { CatalogComponent } from './features/catalog/catalog/catalog.component';
import { LoginComponent } from './features/auth/login/login.component';
export const routes: Routes = [
  { path: '', component: CatalogComponent }, // Trang chủ vào thẳng Catalog
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];