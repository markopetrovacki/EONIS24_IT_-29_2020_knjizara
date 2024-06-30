import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePorudzbinaComponent } from './create-porudzbina.component';

describe('CreatePorudzbinaComponent', () => {
  let component: CreatePorudzbinaComponent;
  let fixture: ComponentFixture<CreatePorudzbinaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreatePorudzbinaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreatePorudzbinaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
