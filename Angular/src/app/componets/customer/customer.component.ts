import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { UserStoreService } from '../../services/user-store.service';
import { Korisnik } from '../../models/korisnik';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent {

  public korisnici: any[] = [];
  public fullName: string ="";
 // public trenutniKorisnik!: any;
  public trenutniKorisnik: Korisnik = {
    id_korisnik: '',
    ime_korisnika: '',
    prezime_korisnika: '',
    adresa_korisnika: '',
    grad_korisnika: '',
    kontakt_telefon: '',
    status_korisnika: ''
  };

  constructor(private auth: AuthService,
    private api : ApiService, 
    private userStore: UserStoreService,
    ){}

    ngOnInit(){
      this.api.getUsers()
      .subscribe(res=>{
        this.korisnici = res;
      });
      
      this.userStore.getFullNameFromStore()
      .subscribe(val=>{
        const fullNameFromToken = this.auth.getFullNameFromToken();
        this.fullName = val || fullNameFromToken;
        console.log('Full Name:', this.fullName);
      });
      
      this.loadUserByUsername();
     
    }

    loadUserByUsername(){
      this.api.getUserByUsername(this.fullName).subscribe((data: Korisnik) => {
        this.trenutniKorisnik = data;
      })
    }
    

    updateTrenutniKorisnik() {
      this.api.updateUser(this.trenutniKorisnik).subscribe(

        res => {
          console.log('Korisnik ažuriran', res);
          alert('Podaci uspešno ažurirani.');
        },
        
        err => { 
          console.error('Greška prilikom ažuriranja korisnika', err);
          alert('Došlo je do greške prilikom ažuriranja.');
        }
        
      ); 
    }
}
