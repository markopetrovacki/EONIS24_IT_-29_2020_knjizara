import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './componets/login/login.component';
import { Component } from '@angular/core';
import { SignupComponent } from './componets/signup/signup.component';
import { DashboardComponent } from './componets/dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { CreateKorisnikComponent } from './componets/dialog/create-korisnik/create-korisnik.component';
import { HeaderComponent } from './componets/header/header.component';
import { ProductsComponent } from './componets/products/products.component';
import { CartComponent } from './componets/cart/cart.component';
import { CustomerComponent } from './componets/customer/customer.component';
import { KnjizaraComponent } from './componets/knjizara/knjizara.component';
import { UserOrdersComponent } from './componets/user-orders/user-orders.component';


const routes: Routes = [
  {path: '', redirectTo: 'login', pathMatch: 'full'},
  {path: 'login', component:LoginComponent},
  {path: 'signup', component:SignupComponent},
  {path: 'dashboard', component:DashboardComponent, canActivate:[AuthGuard]},
  {path: 'cart', component:CartComponent},
  {path: 'products', component:ProductsComponent},
  {path: 'customers', component:CustomerComponent, canActivate:[AuthGuard]},
  {path: 'knjizara', component:KnjizaraComponent},
  {path: 'user-orders', component:UserOrdersComponent, canActivate:[AuthGuard]},
  //{ path: 'create-korisnik', component: CreateKorisnikComponent },
  //{ path: '', redirectTo: '/dashboard', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
