import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateKorisnikComponent } from './create-korisnik.component';

describe('CreateKorisnikComponent', () => {
  let component: CreateKorisnikComponent;
  let fixture: ComponentFixture<CreateKorisnikComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateKorisnikComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateKorisnikComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
