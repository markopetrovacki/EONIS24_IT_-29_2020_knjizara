import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { UserStoreService } from '../../services/user-store.service';
import { ApiService } from '../../services/api.service';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  public fullName: string = '';
  public totalItem : number = 0;
  public searchTerm !: string;
  public role!: string;


  constructor(
    private auth: AuthService,
    private userStore: UserStoreService,
    private cartService : CartService
   
  ) { }

  ngOnInit(): void {
    this.cartService.getProducts()
    .subscribe(res=>{
      this.totalItem = res.length;
    })

    // Pozivamo metodu getFullNameFromStore iz UserStoreService
    this.userStore.getFullNameFromStore().subscribe(val => {
      this.fullName = val || this.auth.getFullNameFromToken();
      console.log('Full Name from UserDashboard:', this.fullName);
    });

    this.userStore.getRoleFromStore()
  .subscribe(val=>{
    const roleFromToken = this.auth.getRoleFromToken();
    this.role = val || roleFromToken;
  });
  }

  // Metoda logout koja koristi AuthService
  logout(): void {
    this.auth.signOut();
    console.log('User logged out from UserDashboard');
  }

  search(event:any){
    this.searchTerm = (event.target as HTMLInputElement).value;
    console.log(this.searchTerm);
    this.cartService.search.next(this.searchTerm);
  }
}
