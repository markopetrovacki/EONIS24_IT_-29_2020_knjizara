import { Component, EventEmitter, Output, Input, input } from '@angular/core';
import { Porudzbina } from '../../../models/porudzbina';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../services/api.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CartComponent } from '../../cart/cart.component';
import { CartService } from '../../../services/cart.service';
import { HttpClient } from '@angular/common/http';
import { StripeError, loadStripe } from '@stripe/stripe-js';
import { ISession } from '../../../models/session';
import { UserStoreService } from '../../../services/user-store.service';
import { AuthService } from '../../../services/auth.service';

declare const Stripe: any;

@Component({
  selector: 'app-create-porudzbina',
  templateUrl: './create-porudzbina.component.html',
  styleUrl: './create-porudzbina.component.css'
})
export class CreatePorudzbinaComponent {
  @Output() porudzbinaCreated  = new EventEmitter<Porudzbina>();
  @Input() porudzbina!: Porudzbina; // Ulazni podatak za ažuriranje korisnika
  @Output() porudzbinaUpdated  = new EventEmitter<Porudzbina>();
  @Input() action!: number; // 1 - Add, 2 - Edit, 3 - Delete
  createPorudzbinaForm: FormGroup;

  @Input() products!: any[];
  @Input() userId!: string;
  @Input() ukupnaCena!: Number;
  @Input() datumKreiranja!: string;
  @Input() statusPorudzbine!: string;
  @Input() statusPlacanja!: string;
  @Input() dobavljac!: string;
  @Input() brojPosiljke!: string;
  @Input() rokIsporuke!: string;
  @Input() idDostava!: string;
  public fullName: string ="";
  public role: string="";

  baseUrl = "http://localhost:5297/api/";

  constructor(private auth: AuthService, private fb: FormBuilder, private apiService: ApiService, public activeModal: NgbActiveModal, public cartService: CartService,  private http: HttpClient,private userStore: UserStoreService,) {
    this.createPorudzbinaForm = this.fb.group({
      datum_kreiranja: ['', Validators.required],
      ukupna_cena: ['', Validators.required],
      status_porudzbine: ['', Validators.required],
      status_placanja: ['', Validators.required],
      dobavljac: ['', Validators.required],
      broj_posiljke: ['', Validators.required],
      rok_isporuke: ['', Validators.required],
      id_korisnik: ['', Validators.required],
      id_dostava: ['', Validators.required]
    });
  }
  
  ngOnInit() {

    this.createPorudzbinaForm.patchValue({
      id_korisnik: this.userId,
      datum_kreiranja: this.datumKreiranja,
      ukupna_cena: this.ukupnaCena,
      status_porudzbine: this.statusPorudzbine,
      status_placanja: this.statusPlacanja,
      dobavljac: this.dobavljac,
      broj_posiljke: this.brojPosiljke,
      rok_isporuke: this.rokIsporuke,
      id_dostava: this.idDostava, 
      // Postavite ostale vrednosti forme prema proizvodima ako je potrebno
    });

    if (this.action === 2 && this.porudzbina) {
     
      this.createPorudzbinaForm.patchValue(this.porudzbina);
    }
    
  }

  getFormattedDate(action: number): string {
    const today = new Date();
    const formattedDate = today.toISOString().split('T')[0]; // Pretvara trenutni datum u format 'yyyy-MM-dd'
    
    return action === 1 ? formattedDate : '';
  }

  onCheckout(): void {   
    const items = this.products.map((product: any) => ({
      naziv_knjige: product.naziv_knjige,
      slika: product.slika,
      ukupna_cena: product.cena,
      quantity: product.kolicina,
    }));this.handleSuccessfulPayment();
    console.log(this.products);
    

    
  
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      this.role = val || roleFromToken;
    });
    /*this.http.post('http://localhost:4242/checkout', { items })     //Ovo je onaj strajp na frontu
      .subscribe(async (res: any) => {
        let stripe = await loadStripe('pk_test_51LUQwuG8zN8FyI0EeQYyTW8AtMHauyRYbg7l5DGZ2VyP1p2PoGqTbBIpIRS1Menv6VzVzXwiiO0gpETr4p6DIrOx00kfS7Zwc4');
        stripe?.redirectToCheckout({
          sessionId: res.id,
        });

        this.handleSuccessfulPayment();
      });*/
  }
 
  requestSession(porudzbinaId:string){
    this.http.post<ISession>(this.baseUrl + 'create-cehckout-session/' + porudzbinaId,{}).subscribe((session)=>{
      this.redirectToCheckout(session.sessionId);
    });
  }

  redirectToCheckout(sessionId: string) {
    const stripe = Stripe('pk_test_51LUQwuG8zN8FyI0EeQYyTW8AtMHauyRYbg7l5DGZ2VyP1p2PoGqTbBIpIRS1Menv6VzVzXwiiO0gpETr4p6DIrOx00kfS7Zwc4');
    stripe.redirectToCheckout({
      sessionId: sessionId
    });
  }


     onSubmit() {   
      if (this.createPorudzbinaForm.valid) {
        const formData = this.createPorudzbinaForm.value;
    
        if (this.action === 1) {
          this.cartService.createOrder(this.createPorudzbinaForm.value);
          this.activeModal.close('Save click');
        } else if (this.action === 2) {
          formData.id_porudzbina = this.porudzbina.id_porudzbina;
          this.apiService.updatePorudzbina(formData).subscribe(
            (updatedPorudzbina: Porudzbina) => {
              this.porudzbinaUpdated.emit(updatedPorudzbina);
              this.activeModal.close();
            },
            error => {
              console.error('Error updating porudzbina', error);
            }
          );
        } else if (this.action === 3) {
           this.onCheckout()
           // this.requestSession( id_porudzbina );
           

           /* this.apiService.createPorudzbina(formData).subscribe(
              (newPorudzbina: Porudzbina) => {
                this.porudzbinaCreated.emit(newPorudzbina);
                this.cartService.createOrder(newPorudzbina);
                this.activeModal.close('Save click');
                this.requestSession(newPorudzbina.id_porudzbina);
              },
              error => {
                console.error('Error creating porudzbina', error);
              }
            );*/
        }
      }
    }

    
    private handleSuccessfulPayment(): void {
      this.cartService.createOrder(this.createPorudzbinaForm.value);
      this.activeModal.close('Save click after checkout');
     
    }


 
}
