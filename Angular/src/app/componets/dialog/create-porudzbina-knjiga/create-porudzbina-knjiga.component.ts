import { Component, EventEmitter, Output, Input} from '@angular/core';
import { PorudzbinaKnjiga } from '../../../models/porudzbinaKnjiga';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../services/api.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-porudzbina-knjiga',
  templateUrl: './create-porudzbina-knjiga.component.html',
  styleUrl: './create-porudzbina-knjiga.component.css'
})
export class CreatePorudzbinaKnjigaComponent {
  @Output() porudzbinaKnjigaCreated  = new EventEmitter<PorudzbinaKnjiga>();
  @Input() porudzbinaKnjiga!: PorudzbinaKnjiga; // Ulazni podatak za ažuriranje korisnika
  @Output() porudzbinaKnjigaUpdated  = new EventEmitter<PorudzbinaKnjiga>();
  @Input() action!: number; // 1 - Add, 2 - Edit, 3 - Delete
  createPorudzbinaKnjigaForm: FormGroup;

  constructor(private fb: FormBuilder, private apiService: ApiService, public activeModal: NgbActiveModal) {
    this.createPorudzbinaKnjigaForm = this.fb.group({
      id_knjige: ['', Validators.required],
      id_porudzbina: ['', Validators.required],
      kolicina: ['', Validators.required],
    });
  }
  
  ngOnInit() {
    if (this.action === 2 && this.porudzbinaKnjiga) {
      this.createPorudzbinaKnjigaForm.patchValue(this.porudzbinaKnjiga);
    }
  }

  onSubmit() {
    if (this.createPorudzbinaKnjigaForm.valid) {
      const formData = this.createPorudzbinaKnjigaForm.value;
      if (this.action === 1) {
        this.apiService.createPorudzbinaKnjiga(formData).subscribe(
          (newPorudzbinaKnjiga: PorudzbinaKnjiga) => {
            this.porudzbinaKnjigaCreated.emit(newPorudzbinaKnjiga);
            this.activeModal.close();
          },
          error => {
            console.error('Error creating porudzbinaKnjiga', error);
          }
        );
      }
    }
  }
}
