import { Component, OnInit, Renderer2 } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { UserStoreService } from '../../services/user-store.service';
import { Router } from '@angular/router';
import { Korisnik } from '../../models/korisnik';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CreateKorisnikComponent } from '../dialog/create-korisnik/create-korisnik.component';
import { CreateDobavljacComponent } from '../dialog/create-dobavljac/create-dobavljac.component';
import { CreateDostavaComponent } from '../dialog/create-dostava/create-dostava.component';
import { CreateKnjigaComponent } from '../dialog/create-knjiga/create-knjiga.component';
import { CreatePorudzbinaComponent } from '../dialog/create-porudzbina/create-porudzbina.component';
import { CreatePorudzbinaKnjigaComponent } from '../dialog/create-porudzbina-knjiga/create-porudzbina-knjiga.component';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{

  public trenutniKorisnik!: any;

  public korisnici: any[] = [];
  public dobavljaci:any[] = [];
  public dostave:any[] = [];
  public knjige:any[] = [];
  public porudzbine:any[] = [];
  public porudzbineById:any = [];
  public porudzbineKnjige:any[] = [];
  public role!: string;
  public searchTerm: string = ''; // Dodajemo searchTerm

  public filteredKorisnici: any[] = [];
  public filteredDobavljaci: any[] = [];
  public filteredDostava: any[] = [];
  public filteredKnjiga: any[] = [];
  public filteredKorisnik: any[] = [];
  public filteredPorudzbina: any[] = [];
  public filteredPorudzbinaKnjiga: any[] = [];


  // Varijable za sortiranje
  public sortKey: string = '';
  public sortDirection: string = 'asc';
  public isSortedAsc: boolean = true;

  public fullName: string ="";
 

  constructor(private auth: AuthService,
              private api : ApiService, 
              private userStore: UserStoreService,
              private router: Router,
              private modalService: NgbModal

              ){}

              
              
ngOnInit(){
  this.api.getUsers()
  .subscribe(res=>{
    this.korisnici = res;
    this.filteredKorisnici = res;
  });
  this.api.getDobavljac().subscribe(res=>{ this.dobavljaci = res; this.filteredDobavljaci = res;});
  this.api.getDosatava().subscribe(res=>{ this.dostave = res; this.filteredDostava = res;});
  this.api.getKnjiga().subscribe(res=>{ this.knjige = res; this.filteredKnjiga = res;});
  this.api.getPorudzbina().subscribe(res=>{ this.porudzbine = res; this.filteredPorudzbina = res;});
  this.api.getPorudzbinaKnjiga().subscribe(res=>{ this.porudzbineKnjige = res;  this.filteredPorudzbinaKnjiga = res;});
  
  this.userStore.getFullNameFromStore()
  .subscribe(val=>{
    const fullNameFromToken = this.auth.getFullNameFromToken();
    this.fullName = val || fullNameFromToken;
    console.log('Full Name:', this.fullName);
  });

  this.userStore.getRoleFromStore()
  .subscribe(val=>{
    const roleFromToken = this.auth.getRoleFromToken();
    this.role = val || roleFromToken;
  });
 
  
  
  this.loadUserByUsername();

}


logout(){
  this.auth.signOut();
  console.log('User logged out from UserDashboard');
}
/*
createUser() {
  this.router.navigate(['/create-korisnik']);
}*/


onUserCreated(user: Korisnik) {
  this.korisnici.push(user);
}
openCreateForm() {
  const modalRef = this.modalService.open(CreateKorisnikComponent);
  modalRef.componentInstance.userCreated.subscribe((newUser: any) => {
    this.onUserCreated(newUser);
  });
}/*
deleteUser(id: string) {
  console.log('Deleting user with id:', id); 
  this.api.deleteUser(id).subscribe(
    res => {
      console.log('User deleted:', res);
      this.korisnici = this.korisnici.filter((korisnik: any) => korisnik.id !== id);
     
    },
    err => {
      console.error('Error deleting user:', err);
    }
  );
}/*
updateUser(updatedUser: Korisnik) {
  console.log('Updating user:', updatedUser);
  this.api.updateUser(updatedUser).subscribe(
    res => {
      console.log('User updated:', res); // Check response from server
      // Find the index of the updated user in the korisnici array
      const index = this.korisnici.findIndex((user: any) => user.id === updatedUser.id_korisnik);
      if (index !== -1) {
        // Update the user in the korisnici array
        this.korisnici[index] = updatedUser;
        //this.cdr.detectChanges(); // Force Angular to detect changes
      }
    },
    err => {
      console.error('Error updating user:', err); // Check for errors
    }
  );
}*/

  
  /*loadUsers() {
    this.api.getUsers().subscribe((data: any) => {
      if (this.searchTerm.trim() === '') {
        this.korisnici = data;
      } else {
        this.korisnici = data.filter((user: any) =>
          Object.values(user).some(value =>
            String(value).toLowerCase().includes(this.searchTerm.toLowerCase())
          )
        );
      }
    });
  }*/

  loadUserByUsername(){
    this.api.getUserByUsername(this.fullName).subscribe((data: Korisnik) => {
      this.trenutniKorisnik = data;
      // Nakon što je trenutni korisnik uspešno dobijen, učitaj porudžbine za tog korisnika
      this.loadPorudzbinaByIdKorisnik();
    })
  }

  loadUsers() {

    this.api.getUsers().subscribe(res=>{ this.korisnici = res; this.filteredKorisnici = res;});
    
  }

  openDialog(action: number, korisnik?: any) {
    const modalRef = this.modalService.open(CreateKorisnikComponent);
    modalRef.componentInstance.action = action;

    if (action === 2 && korisnik) {
      modalRef.componentInstance.korisnik = korisnik;
    }

    modalRef.componentInstance.userUpdated.subscribe((updatedUser: any) => {
      this.loadUsers();
    });

    modalRef.componentInstance.userCreated.subscribe((newUser: any) => {
      this.loadUsers();
    });
  }
  deleteUser(id: string) {
    this.api.deleteUser(id).subscribe(
      () => {
        this.loadUsers();
      },
      err => {
        console.error('Error deleting user:', err);
      }
    );
  }



  loadDobavljac() {

    this.api.getDobavljac().subscribe(res=>{ this.dobavljaci = res; this.filteredDobavljaci = res;});

  }
  openDialogDobavljac(action: number, dobavljac?: any) {
    const modalRef = this.modalService.open(CreateDobavljacComponent);
    modalRef.componentInstance.action = action;

    if (action === 2 && dobavljac) {
      modalRef.componentInstance.dobavljac = dobavljac;
    }

    modalRef.componentInstance.dobavljacUpdated.subscribe((updateDobavljac: any) => {
      this.loadDobavljac();
     
    });

    modalRef.componentInstance.dobavljacCreated.subscribe((newDobavljac: any) => {
      this.loadDobavljac();
     
    });
  }
  deleteDobavljac(id: string) {
    this.api.deleteDobavljac(id).subscribe(
      () => {
        this.loadDobavljac();
        
      },
      err => {
        console.error('Error deleting user:', err);
      }
    );
  }



  loadDostava() {

    this.api.getDosatava().subscribe(res=>{ this.dostave = res; this.filteredDostava = res;});
 
  }
  openDialogDostava(action: number, dostava?: any) {
    const modalRef = this.modalService.open(CreateDostavaComponent);
    modalRef.componentInstance.action = action;

    if (action === 2 && dostava) {
      modalRef.componentInstance.dostava = dostava;
    }
                              
    modalRef.componentInstance.dostavaUpdated.subscribe((updatedDostava: any) => {
      this.loadDostava();
    });

    modalRef.componentInstance.dostavaCreated.subscribe((newDostava: any) => {
      this.loadDostava();
    });
  }
  deleteDostava(id: string) {
    this.api.deleteDostava(id).subscribe(
      () => {
        this.loadDostava();
      },
      err => {
        console.error('Error deleting dostava:', err);
      }
    );
  }


  loadKnjiga() {

    this.api.getKnjiga().subscribe(res=>{ this.knjige = res; this.filteredKnjiga = res;});
  
  }
  openDialogKnjiga(action: number, knjiga?: any) {
    const modalRef = this.modalService.open(CreateKnjigaComponent);
    modalRef.componentInstance.action = action;

    if (action === 2 && knjiga) {
      modalRef.componentInstance.knjiga = knjiga;
    }

    modalRef.componentInstance.knjigaUpdated.subscribe((updatedKnjiga: any) => {
      this.loadKnjiga();
    });

    modalRef.componentInstance.knjigaCreated.subscribe((newKnjiga: any) => {
      this.loadKnjiga();
    });
  }
  deleteKnjiga(id: string) {
    this.api.deleteKnjiga(id).subscribe(
      () => {
        this.loadKnjiga();
      },
      err => {
        console.error('Error deleting knjiga:', err);
      }
    );
  }


  loadPorudzbina() {

    this.api.getPorudzbina().subscribe(res=>{ this.porudzbine = res; this.filteredPorudzbina = res;});
    
  }
  
  loadPorudzbinaByIdKorisnik() {
    if (!this.trenutniKorisnik || !this.trenutniKorisnik.id_korisnik) {
      console.error('Trenutni korisnik ili njegov ID nije definisan');
      return;
    }
    const id = this.trenutniKorisnik.id_korisnik;
    this.api.getPorudzbinaByIdKorisnik(id).subscribe(
      (data: any) => {
        if (Array.isArray(data)) {
          this.porudzbineById = data;
        } else {
          console.error('Očekivan niz, ali dobijen objekat:', data);
          this.porudzbineById = [data]; // Dodajemo objekat u niz jednočlanog objekta
        }
      },
      (error) => {
        console.error('Greška prilikom dobijanja porudžbina po ID korisnika:', error);
        this.porudzbineById = []; // ili setujte na prazan niz u slučaju greške
      }
    );

  }
  /*loadPorudzbinaByIdKorisnik() {
    if (!this.trenutniKorisnik || !this.trenutniKorisnik.id_korisnik) {
      console.error('Trenutni korisnik ili njegov ID nije definisan');
      return;
    }
    const id = this.trenutniKorisnik.id_korisnik;
    this.api.getPorudzbinaByIdKorisnik(id).subscribe((data: any) => {
      this.porudzbineById = data;
    });
    /*const id = this.trenutniKorisnik.id_korisnik;
    this.api.getPorudzbinaByIdKorisnik(id).subscribe((data: any) => {
      this.porudzbineById = data;
    });*/
 // }

  openDialogPorudzbina(action: number, porudzbina?: any) {
    const modalRef = this.modalService.open(CreatePorudzbinaComponent);
    modalRef.componentInstance.action = action;

    if (action === 2 && porudzbina) {
      modalRef.componentInstance.porudzbina = porudzbina;
    }

    modalRef.componentInstance.porudzbinaUpdated.subscribe((updatedPorudzbina: any) => {
      this.loadPorudzbina();
    });

    modalRef.componentInstance.porudzbinaCreated.subscribe((newPorudzbina: any) => {
      this.loadPorudzbina();
    });
  }
 /* deletePorudzbina(id: string) {
    this.api.deletePorudzbinaKnjigaByPorudzbinaId(id).subscribe();
    
    this.api.deletePorudzbina(id).subscribe(
      () => {
        console.log(`Deleting porudzbina with id: ${id}`);
        this.loadPorudzbina();
      },
      err => {
        console.error('Error deleting porudzbina:', err);
      }
    );
  }*/

  deletePorudzbina(id: string) {
    // Prvo proveravamo da li postoje stavke porudzbinaKnjiga za datu porudžbinu
    this.api.getKnjigeByPorudzbinaId(id).subscribe(
      (porudzbinaKnjiga) => {
        if (porudzbinaKnjiga && porudzbinaKnjiga.length > 0) {
          // Postoje stavke, prvo ih brišemo
          this.api.deletePorudzbinaKnjigaByPorudzbinaId(id).subscribe(
            () => {
              console.log(`Deleted porudzbinaKnjiga entries for porudzbina with id: ${id}`);
              
              // Nakon uspešnog brisanja porudzbinaKnjiga, brišemo samu porudžbinu
              this.api.deletePorudzbina(id).subscribe(
                () => {
                  console.log(`Deleted porudzbina with id: ${id}`);
                  this.loadPorudzbina(); // Ponovno učitavanje porudžbina nakon brisanja
                },
                err => {
                  console.error('Error deleting porudzbina:', err);
                }
              );
            },
            err => {
              console.error('Error deleting porudzbinaKnjiga entries:', err);
            }
          );
        } else {
          // Ne postoje stavke porudzbinaKnjiga, direktno brišemo porudžbinu
          this.api.deletePorudzbina(id).subscribe(
            () => {
              console.log(`Deleted porudzbina with id: ${id}`);
              this.loadPorudzbina(); // Ponovno učitavanje porudžbina nakon brisanja
            },
            err => {
              console.error('Error deleting porudzbina:', err);
            }
          );
        }
      },
      err => {
        console.error('Error retrieving porudzbinaKnjiga entries:', err);
      }
    );
  }
  


  loadPorudzbinaKnjiga() {

    this.api.getPorudzbinaKnjiga().subscribe(res=>{ this.porudzbineKnjige = res;  this.filteredPorudzbinaKnjiga = res;});

  }
  openDialogPorudzbinaKnjiga(action: number, getPorudzbinaKnjiga?: any) {
    const modalRef = this.modalService.open(CreatePorudzbinaKnjigaComponent);
    modalRef.componentInstance.action = action;

    modalRef.componentInstance.porudzbinaKnjigaCreated.subscribe((newPorudzbinaKnjiga: any) => {
      this.loadPorudzbinaKnjiga();
    });
  }
  deletePorudzbinaKnjiga(id_knjige: string, id_porudzbina: string): void {
    this.api.deletePorudzbinaKnjiga(id_knjige, id_porudzbina).subscribe(
      () => {
        this.loadPorudzbinaKnjiga();
      },
      err => {
        console.error('Error deleting porudzbinaKnjiga:', err);
      }
    );
  }

  

  filterTables() {
    const searchTermLower = this.searchTerm.toLowerCase();

    this.filteredKorisnici = this.korisnici.filter(korisnik =>
      Object.values(korisnik).some(value =>
        String(value).toLowerCase().includes(searchTermLower)
      )
    );
  
    this.filteredDobavljaci = this.dobavljaci.filter(dobavljac =>
      Object.values(dobavljac).some(value =>
        String(value).toLowerCase().includes(searchTermLower)
      )
    );

    this.filteredDostava = this.dostave.filter(dostava =>
      Object.values(dostava).some(value =>
        String(value).toLowerCase().includes(searchTermLower)
      )
    );

    this.filteredKnjiga = this.knjige.filter(knjiga =>
      Object.values(knjiga).some(value =>
        String(value).toLowerCase().includes(searchTermLower)
      )
    );

    this.filteredPorudzbina = this.porudzbine.filter(porudzbina =>
      Object.values(porudzbina).some(value =>
        String(value).toLowerCase().includes(searchTermLower)
      )
    );

    this.filteredPorudzbinaKnjiga = this.porudzbineKnjige.filter(porudzbinaKnjiga =>
      Object.values(porudzbinaKnjiga).some(value =>
        String(value).toLowerCase().includes(searchTermLower)
      )
    );
  }

  sortData(key: string) {
    this.isSortedAsc = !this.isSortedAsc;
    this.sortKey = key;
    this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';

    const direction = this.sortDirection === 'asc' ? 1 : -1;
    const compare = (a: any, b: any) => {
      if (a[key] < b[key]) return -1 * direction;
      if (a[key] > b[key]) return 1 * direction;
      
      return 0;
    };

    this.filteredKorisnici.sort(compare);
    this.filteredDobavljaci.sort(compare);
    this.filteredDostava.sort(compare);
    this.filteredKnjiga.sort(compare);
    this.filteredPorudzbina.sort(compare);
    this.filteredPorudzbinaKnjiga.sort(compare);
  }
  
  getSortIcon(column: string): string {
    if (this.sortKey === column) {
      return this.isSortedAsc ? '&#9650;' : '&#9660;'; // ▲ : ▼
    }
    return '';

  }


}
