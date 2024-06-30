import { Component, EventEmitter, Output, Input } from '@angular/core';
import { Knjiga } from '../../../models/knjiga';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../services/api.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-knjiga',
  templateUrl: './create-knjiga.component.html',
  styleUrl: './create-knjiga.component.css'
})
export class CreateKnjigaComponent {

  @Output() knjigaCreated  = new EventEmitter<Knjiga>();
  @Input() knjiga!: Knjiga; // Ulazni podatak za ažuriranje korisnika
  @Output() knjigaUpdated  = new EventEmitter<Knjiga>();
  @Input() action!: number; // 1 - Add, 2 - Edit, 3 - Delete
  createKnjigaForm: FormGroup;

  constructor(private fb: FormBuilder, private apiService: ApiService, public activeModal: NgbActiveModal) {
    this.createKnjigaForm = this.fb.group({
      naziv_knjige: ['', Validators.required],
      opis: ['', Validators.required],
      stanje_na_lageru: ['', Validators.required],
      cena: ['', Validators.required],
      zanr: ['', Validators.required],
      ime_autora: ['', Validators.required],
      prezime_autora: ['', Validators.required],
      slika: ['', Validators.required],
      id_dobavljac: ['', Validators.required]
    });
  }
  
  ngOnInit() {
    if (this.action === 2 && this.knjiga) {
      this.createKnjigaForm.patchValue(this.knjiga);
    }
  }

  onSubmit() {
    if (this.createKnjigaForm.valid) {
      const formData = this.createKnjigaForm.value;
      if (this.action === 1) {
        this.apiService.createKnjiga(formData).subscribe(
          (newKnjiga: Knjiga) => {
            this.knjigaCreated.emit(newKnjiga);
            this.activeModal.close();
          },
          error => {
            console.error('Error creating knjiga', error);
          }
        );
      } else if (this.action === 2) {
        formData.id_knjige = this.knjiga.id_knjige;
        this.apiService.updateKnjiga(formData).subscribe(
          (updatedKnjiga: Knjiga) => {
            this.knjigaUpdated.emit(updatedKnjiga);
            this.activeModal.close();
          },
          error => {
            console.error('Error updating knjiga', error);
          }
        );
      }
    }
  }


}
