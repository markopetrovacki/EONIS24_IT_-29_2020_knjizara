import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateKnjigaComponent } from './create-knjiga.component';

describe('CreateKnjigaComponent', () => {
  let component: CreateKnjigaComponent;
  let fixture: ComponentFixture<CreateKnjigaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateKnjigaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateKnjigaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
