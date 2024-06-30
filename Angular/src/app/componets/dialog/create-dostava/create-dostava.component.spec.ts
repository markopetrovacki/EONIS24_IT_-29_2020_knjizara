import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDostavaComponent } from './create-dostava.component';

describe('CreateDostavaComponent', () => {
  let component: CreateDostavaComponent;
  let fixture: ComponentFixture<CreateDostavaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateDostavaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateDostavaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
