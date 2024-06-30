import { Component } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { UserStoreService } from '../../services/user-store.service';
import { Korisnik } from '../../models/korisnik';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CreatePorudzbinaComponent } from '../dialog/create-porudzbina/create-porudzbina.component';
import { formatDate } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { loadStripe } from '@stripe/stripe-js';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {

  public products : any = [];
  public grandTotal !: number;

  public trenutniKorisnik!: any;
  public fullName: string ="";

  public dateKreiranja!:string;
  public dateDostave!:string;
 

  constructor(private cartService : CartService,
              private auth: AuthService,
              private api : ApiService, 
              private userStore: UserStoreService,
              private modalService: NgbModal,
              private http: HttpClient) { }

  ngOnInit(): void {
    this.cartService.getProducts()
    .subscribe(res=>{
      this.products = res;
      this.grandTotal = this.cartService.getTotalPrice();
      
    })

    this.userStore.getFullNameFromStore()
    .subscribe(val=>{
      const fullNameFromToken = this.auth.getFullNameFromToken();
      this.fullName = val || fullNameFromToken;
      console.log('Full Name:', this.fullName);

    });
  
    
    this.loadUserByUsername();
    this.printAllBookIds();
  }

  loadUserByUsername(){
    this.api.getUserByUsername(this.fullName).subscribe((data: Korisnik) => {
      this.trenutniKorisnik = data;
      // Nakon što je trenutni korisnik uspešno dobijen, učitaj porudžbine za tog korisnika
      console.log('Id_korisnika', this.trenutniKorisnik.id_korisnik)
      
    })  
  }

  removeItem(item: any){
    this.cartService.removeCartItem(item);
  }
  emptycart(){
    this.cartService.removeAllCart();
  }

  // Nova metoda za ispis svih id_knjiga
  printAllBookIds() {
    for (let product of this.products) {
      console.log(product.id_knjige);
    }
  }
 
    // Povećavanje količine
 /* increaseQuantity(product: any) {
    product.kolicina++;
    product.total = product.kolicina * product.cena;
    this.cartService.updateQuantity(product, product.kolicina);
    this.updateTotalPrice();
  }*/

    // Povećavanje količine sa proverom dostupnosti
  increaseQuantity(product: any) {
    // Proverite da li je dostupna količina veća ili jednaka trenutnoj količini plus jedan
    if (product.kolicina < product.stanje_na_lageru) {
      product.kolicina++;
      product.total = product.kolicina * product.cena;
      this.cartService.updateQuantity(product, product.kolicina);
      this.updateTotalPrice();
    } else {
      // Prikaz obaveštenja ukoliko pokušavaju da dodaju više nego što je dostupno
      alert("Nema dovoljno artikala na lageru.");
      console.log('Nema dovoljno artikala na lageru.');
    }
  }

  // Smanjivanje količine
  decreaseQuantity(product: any) {
    if (product.kolicina > 1) {
      product.kolicina--;
      product.total = product.kolicina * product.cena;
      this.cartService.updateQuantity(product, product.kolicina);
      this.updateTotalPrice();
    }
  }
  updateTotalPrice() {
    this.grandTotal = this.cartService.getTotalPrice();
  }

  createDate() {
    // Trenutni datum
    const today = new Date();
    const datum_kreiranja = formatDate(today, 'yyyy-MM-dd', 'en-US');
    const datum_dostave = formatDate(new Date(today.setDate(today.getDate() + 3)), 'yyyy-MM-dd', 'en-US');
   // const id_dostava = Math.floor(Math.random() * 1000); // Generisanje nasumičnog broja za id_dostava
   
  /* this.dateKreiranja = new Date(datum_kreiranja);
   this.dateDostave= new Date(datum_dostave);*/

   this.dateKreiranja = datum_kreiranja;
   this.dateDostave = datum_dostave;
    // Kreiranje objekta porudžbine
   /* const newOrder = {
      datum_kreiranja: dateKreiranja,
      ukupna_cena: this.getTotalPrice(),
      status_porudzbine: 'Na čekanju',
      status_placanja: 'Neplaćeno',
      dobavljac: 'N/A',
      broj_posiljke: 'N/A',
      rok_isporuke: dateDostave,
      id_korisnik: id_korisnika,
      id_dostava: '4E4BBBA9-9DBE-4F8D-847A-310E08D26DD1'
    };

    // Pozivanje API servisa za kreiranje porudžbine
    this.api.createPorudzbina(newOrder).subscribe(
      (response) => {
        console.log('Porudžbina kreirana:', response);
        this.removeAllCart(); // Isprazni korpu nakon kreiranja porudžbine
      },
      (error) => {
        console.error('Greška pri kreiranju porudžbine:', error);
      }
    );*/
  }

  generateRandomSixDigitNumber(): string {
    return Math.floor(100000 + Math.random() * 900000).toString();
  }

  openCheckoutModal(action: number) {
    this.createDate();
    const modalRef = this.modalService.open(CreatePorudzbinaComponent, { centered: true });
    modalRef.componentInstance.action = action;
    modalRef.componentInstance.products = this.products; // Prosleđivanje proizvoda u formu
    modalRef.componentInstance.userId = this.trenutniKorisnik.id_korisnik; // Prosleđivanje id korisnika u formu
    modalRef.componentInstance.datumKreiranja = this.dateKreiranja;
    modalRef.componentInstance.rokIsporuke = this.dateDostave;
    modalRef.componentInstance.ukupnaCena = this.grandTotal;
    modalRef.componentInstance.statusPorudzbine = 'Na čeknaju';
    modalRef.componentInstance.statusPlacanja = 'Neplaceno';
    modalRef.componentInstance.dobavljac = 'City express';
    modalRef.componentInstance.brojPosiljke = this.generateRandomSixDigitNumber(); //'583246';
    modalRef.componentInstance.idDostava = '4E4BBBA9-9DBE-4F8D-847A-310E08D26DD1';
    modalRef.componentInstance.kolicina = this.products.kolicina;
    
  }
  
 /* onCheckout(): void {
    this.http
      .post('http://localhost:4242/checkout', {
        items: this.products.items,
      })
      .subscribe(async (res: any) => {
        let stripe = await loadStripe('pk_test_51LUQwuG8zN8FyI0EeQYyTW8AtMHauyRYbg7l5DGZ2VyP1p2PoGqTbBIpIRS1Menv6VzVzXwiiO0gpETr4p6DIrOx00kfS7Zwc4');
        stripe?.redirectToCheckout({
          sessionId: res.id,
        });
      });
  }*/
  /*onCheckout(): void {
    const items = this.products.map((product: any) => ({
      naziv_knjige: product.naziv_knjige,
      slika: product.slika,
      ukupna_cena: product.cena,
      quantity: product.kolicina,
    }));
    console.log(this.products);
    this.http.post('http://localhost:4242/checkout', { items })
      .subscribe(async (res: any) => {
        let stripe = await loadStripe('pk_test_51LUQwuG8zN8FyI0EeQYyTW8AtMHauyRYbg7l5DGZ2VyP1p2PoGqTbBIpIRS1Menv6VzVzXwiiO0gpETr4p6DIrOx00kfS7Zwc4');
        stripe?.redirectToCheckout({
          sessionId: res.id,
        });
      });
  }*/

 
}
