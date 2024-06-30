import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePorudzbinaKnjigaComponent } from './create-porudzbina-knjiga.component';

describe('CreatePorudzbinaKnjigaComponent', () => {
  let component: CreatePorudzbinaKnjigaComponent;
  let fixture: ComponentFixture<CreatePorudzbinaKnjigaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreatePorudzbinaKnjigaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreatePorudzbinaKnjigaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
