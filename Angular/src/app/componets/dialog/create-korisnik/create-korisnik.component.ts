import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../services/api.service';
import { Korisnik } from '../../../models/korisnik';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-korisnik',
  templateUrl: './create-korisnik.component.html',
  styleUrl: './create-korisnik.component.css'
})
export class CreateKorisnikComponent {
  @Output() userCreated  = new EventEmitter<Korisnik>();
  @Input() korisnik!: Korisnik; // Ulazni podatak za ažuriranje korisnika
  @Output() userUpdated  = new EventEmitter<Korisnik>();
  @Input() action!: number; // 1 - Add, 2 - Edit, 3 - Delete
  createKorisnikForm: FormGroup;

  constructor(private fb: FormBuilder, private apiService: ApiService, public activeModal: NgbActiveModal) {
    this.createKorisnikForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      ime_korisnika: ['', Validators.required],
      prezime_korisnika: ['', Validators.required],
      adresa_korisnika: ['', Validators.required],
      grad_korisnika: ['', Validators.required],
      kontakt_telefon: ['', Validators.required],
      status_korisnika: ['', Validators.required]
    });
  }
  
  ngOnInit() {
    if (this.action === 2 && this.korisnik) {
      this.createKorisnikForm.patchValue(this.korisnik);
    }
  }

  onSubmit() {
    if (this.createKorisnikForm.valid) {
      const formData = this.createKorisnikForm.value;
      if (this.action === 1) {
        this.apiService.createUser(formData).subscribe(
          (newKorisnik: Korisnik) => {
            this.userCreated.emit(newKorisnik);
            this.activeModal.close();
          },
          error => {
            console.error('Error creating user', error);
          }
        );
      } else if (this.action === 2) {
        formData.id_korisnik = this.korisnik.id_korisnik;
        this.apiService.updateUser(formData).subscribe(
          (updatedKorisnik: Korisnik) => {
            this.userUpdated.emit(updatedKorisnik);
            this.activeModal.close();
          },
          error => {
            console.error('Error updating user', error);
          }
        );
      }
    }
  }

  
}
