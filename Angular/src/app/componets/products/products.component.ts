import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { UserStoreService } from '../../services/user-store.service';
import { CartService } from '../../services/cart.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent {

  public fullName: string ="";
  public knjige:any[] = [];
  
  public filterZanr: any[] = []; // Inicijalizacija kao prazan niz
  //public filterZanr : any
  searchKey:string ="";

  public currentPage: number = 1;
  public itemsPerPage: number = 5; // broj knjiga po stranici
  public currentKnjige: any[] = []; // Knjige za trenutnu stranicu

  constructor(
    private auth: AuthService,
    private api : ApiService, 
    private userStore: UserStoreService,
    public cartService : CartService
    ) {}


  ngOnInit(){
    this.api.getKnjiga()
    .subscribe(res=>{
      this.knjige = res;
      this.filterZanr = res;
      this.updateCurrentKnjige(); 
      this.knjige.forEach((a:any) => {
        
        Object.assign(a,{quantity:1,total:a.price});
      });
      console.log(this.knjige) 
    });
    this.cartService.search.subscribe((val:any)=>{
      this.searchKey = val;
    })
    
    this.userStore.getFullNameFromStore()
    .subscribe(val=>{
      const fullNameFromToken = this.auth.getFullNameFromToken();
      this.fullName = val || fullNameFromToken;
      console.log('Full Name:', this.fullName);
    });
   
  }

 /* addtocart(item: any){
    this.cartService.addtoCart(item);
  }*/
  addtocart(item: any) {
    const cartQuantity = this.cartService.getCartQuantity(item.id_knjige);
    if (item.stanje_na_lageru > cartQuantity) {
      this.cartService.addtoCart(item);
    } else {
      // Opcionalno: prikazivanje poruke korisniku
      alert('Nema dovoljno artikala na lageru.');
    }
  }

  filter(zanr: string) {
    this.currentPage = 1; // Resetuj na prvu stranicu
    this.filterZanr = this.knjige
      .filter((a: any) => {
        if (a.zanr === zanr || zanr === '') {
          return a;
        }
      });
    this.updateCurrentKnjige(); // Ažuriraj podatke za prvu stranicu nakon filtera
  }

  updateCurrentKnjige() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.currentKnjige = this.filterZanr.slice(start, end);
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updateCurrentKnjige(); // Ažuriraj podatke kada se promeni stranica
    }
  }

  get totalPages(): number {
    return Math.ceil(this.filterZanr.length / this.itemsPerPage);
  }

  get totalPagesArray(): number[] {
    return Array(this.totalPages).fill(0).map((x, i) => i + 1);
  }

}
