import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Dobavljac } from '../../../models/dobavljac';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../services/api.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-create-dobavljac',
  templateUrl: './create-dobavljac.component.html',
  styleUrl: './create-dobavljac.component.css'
})
export class CreateDobavljacComponent {

  @Output() dobavljacCreated  = new EventEmitter<Dobavljac>();
  @Input() dobavljac!: Dobavljac; // Ulazni podatak za ažuriranje 
  @Output() dobavljacUpdated  = new EventEmitter<Dobavljac>();
  @Input() action!: number; // 1 - Add, 2 - Edit, 3 - Delete
  createDobavljacForm: FormGroup;


  constructor(private fb: FormBuilder, private apiService: ApiService, public activeModal: NgbActiveModal) {
    this.createDobavljacForm = this.fb.group({
      naziv_dobavljaca: ['', Validators.required],
      pib: ['', Validators.required],
      telefon_dobavljaca: ['', Validators.required],
      adresa_dobavljaca: ['', Validators.required],
      
    });
  }



  ngOnInit() {
    if (this.action === 2 && this.dobavljac) {
      this.createDobavljacForm.patchValue(this.dobavljac);
    }
  }

  onSubmit() {
    if (this.createDobavljacForm.valid) {
      const formData = this.createDobavljacForm.value;
      if (this.action === 1) {
        this.apiService.createDobavljac(formData).subscribe(
          (newDobavljac: Dobavljac) => {
            this.dobavljacCreated.emit(newDobavljac);
            this.activeModal.close();
          },
          error => {
            console.error('Error creating dobavljac', error);
          }
        );
      } else if (this.action === 2) {
        formData.id_dobavljaca = this.dobavljac.id_dobavljaca;
        this.apiService.updateDobavljac(formData).subscribe(
          (updateDobavljac: Dobavljac) => {
            this.dobavljacUpdated.emit(updateDobavljac);
            this.activeModal.close();
          },
          error => {
            console.error('Error updating dobavljac', error);
          }
        );
      }
    }
  }
}
