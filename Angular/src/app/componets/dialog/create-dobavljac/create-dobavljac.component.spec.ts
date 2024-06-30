import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDobavljacComponent } from './create-dobavljac.component';

describe('CreateDobavljacComponent', () => {
  let component: CreateDobavljacComponent;
  let fixture: ComponentFixture<CreateDobavljacComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateDobavljacComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateDobavljacComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
