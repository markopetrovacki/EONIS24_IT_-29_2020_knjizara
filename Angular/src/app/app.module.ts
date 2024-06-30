import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './componets/login/login.component';
import { SignupComponent } from './componets/signup/signup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';
import { DashboardComponent } from './componets/dashboard/dashboard.component';
import { NgToastModule } from 'ng-angular-popup'; // to be added
import { TokenInterceptor } from './interceptors/token.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { CreateKorisnikComponent } from './componets/dialog/create-korisnik/create-korisnik.component';
//import { ItemTableComponent } from './item-table/item-table.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CreateDobavljacComponent } from './componets/dialog/create-dobavljac/create-dobavljac.component';
import { CreateDostavaComponent } from './componets/dialog/create-dostava/create-dostava.component';
import { CreateKnjigaComponent } from './componets/dialog/create-knjiga/create-knjiga.component';
import { CreatePorudzbinaComponent } from './componets/dialog/create-porudzbina/create-porudzbina.component';
import { CreatePorudzbinaKnjigaComponent } from './componets/dialog/create-porudzbina-knjiga/create-porudzbina-knjiga.component';
import { HeaderComponent } from './componets/header/header.component';
import { CartComponent } from './componets/cart/cart.component';
import { ProductsComponent } from './componets/products/products.component';
import { FilterPipe } from './shared/filter.pipe';
import { CustomerComponent } from './componets/customer/customer.component';
import { KnjizaraComponent } from './componets/knjizara/knjizara.component';
import { UserOrdersComponent } from './componets/user-orders/user-orders.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    DashboardComponent,
    CreateKorisnikComponent,
    CreateDobavljacComponent,
    CreateDostavaComponent,
    CreateKnjigaComponent,
    CreatePorudzbinaComponent,
    CreatePorudzbinaKnjigaComponent,
    HeaderComponent,
    CartComponent,
    ProductsComponent,
    FilterPipe,
    CustomerComponent,
    KnjizaraComponent,
    UserOrdersComponent,
    
    //ItemTableComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgToastModule,
    NgbModule,
    FormsModule
    

  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi:true,
    //provideHttpClient(withFetch()),
    //provideClientHydration()
    },
    provideHttpClient(withFetch()),
    //provideClientHydration()
    provideAnimationsAsync()],
  
  bootstrap: [AppComponent]
})
export class AppModule { }
