import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Dostava } from '../../../models/dostava';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../services/api.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-dostava',
  templateUrl: './create-dostava.component.html',
  styleUrl: './create-dostava.component.css'
})
export class CreateDostavaComponent {
  @Output() dostavaCreated  = new EventEmitter<Dostava>();
  @Input() dostava!: Dostava; // Ulazni podatak za ažuriranje korisnika
  @Output() dostavaUpdated  = new EventEmitter<Dostava>();
  @Input() action!: number; // 1 - Add, 2 - Edit, 3 - Delete
  createDostavaForm: FormGroup;

  constructor(private fb: FormBuilder, private apiService: ApiService, public activeModal: NgbActiveModal) {
    this.createDostavaForm = this.fb.group({
      adresa_dostave: ['', Validators.required],
      datum_dostave: ['', Validators.required],
      grad: ['', Validators.required],
      drzava: ['', Validators.required],
      postanski_broj: ['', Validators.required],
      broj_telefona: ['', Validators.required],
      ime: ['', Validators.required],
    });
  }
  
  ngOnInit() {
    if (this.action === 2 && this.dostava) {
      this.createDostavaForm.patchValue(this.dostava);
    }
  }

  onSubmit() {
    if (this.createDostavaForm.valid) {
      const formData = this.createDostavaForm.value;
      if (this.action === 1) {
        this.apiService.createDostava(formData).subscribe(
          (newDostava: Dostava) => {
            this.dostavaCreated.emit(newDostava);
            this.activeModal.close();
          },
          error => {
            console.error('Error creating dostava', error);
          }
        );
      } else if (this.action === 2) {
        formData.id_dostava = this.dostava.id_dostava;
        this.apiService.updateDostava(formData).subscribe(
          (updateDostava: Dostava) => {
            this.dostavaUpdated.emit(updateDostava);
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
