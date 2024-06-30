import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { UserStoreService } from '../../services/user-store.service';
import { Korisnik } from '../../models/korisnik';

@Component({
  selector: 'app-user-orders',
  templateUrl: './user-orders.component.html',
  styleUrl: './user-orders.component.css'
})
export class UserOrdersComponent implements OnInit{
  
  public fullName: string ="";
  public trenutniKorisnik!: any;

  public porudzbine: any[] = [];
  public trenutniKorisnikId!: string;
  public groupedOrders: any[] = [];

  constructor(
    private apiService: ApiService,
    private auth: AuthService,
    private userStore: UserStoreService
  ) { }

  ngOnInit(): void {

    this.userStore.getFullNameFromStore()
    .subscribe(val=>{
      const fullNameFromToken = this.auth.getFullNameFromToken();
      this.fullName = val || fullNameFromToken;
      console.log('Full Name:', this.fullName);
    });

    this.loadUserByUsername();

    /*this.userStore.getUserIdFromStore().subscribe((userId: string) => {
      this.trenutniKorisnikId = userId;
      this.loadOrders();
    });*/
  }

  loadUserByUsername(){
    this.apiService.getUserByUsername(this.fullName).subscribe((data: Korisnik) => {
      this.trenutniKorisnik = data;
      this.trenutniKorisnikId = this.trenutniKorisnik.id_korisnik;
      // Nakon što je trenutni korisnik uspešno dobijen, učitaj porudžbine za tog korisnika
      this.loadOrders();
    })
  }

  loadOrders() {
    this.apiService.getPorudzbinaByIdKorisnik(this.trenutniKorisnikId).subscribe((porudzbine: any[]) => {
      this.porudzbine = porudzbine;
      this.groupOrdersWithBooks();
    });
  }
/*
  groupOrdersWithBooks() {
    this.groupedOrders = [];
    for (const porudzbina of this.porudzbine) {
      this.apiService.getKnjigeByPorudzbinaId(porudzbina.id_porudzbina).subscribe((knjige: any[]) => {
        this.groupedOrders.push({
          ...porudzbina,
          knjige: knjige
        });


      });
    }
  }*/
      getNazivKnjige(id_knjige: string): Promise<string> {
        return new Promise((resolve, reject) => {
          this.apiService.getKnjigaById(id_knjige).subscribe((knjigaDetalji: any) => {
            resolve(knjigaDetalji.naziv_knjige);
          }, (error) => {
            reject(error);
          });
        });
      }
    
      async groupOrdersWithBooks() {
        this.groupedOrders = [];
        for (const porudzbina of this.porudzbine) {
          this.apiService.getKnjigeByPorudzbinaId(porudzbina.id_porudzbina).subscribe(async (knjige: any[]) => {
            const knjigeWithNames = [];
            
            for (const knjiga of knjige) {
              try {
                const naziv_knjige = await this.getNazivKnjige(knjiga.id_knjige);
                knjigeWithNames.push({
                  ...knjiga,
                  naziv_knjige: naziv_knjige
                });
              } catch (error) {
                console.error(`Error fetching book name for id ${knjiga.id_knjige}`, error);
              }
            }
        
            this.groupedOrders.push({
              ...porudzbina,
              knjige: knjigeWithNames
            });
          });
        }
      }



}
