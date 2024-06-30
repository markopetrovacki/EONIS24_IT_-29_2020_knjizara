import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KnjizaraComponent } from './knjizara.component';

describe('KnjizaraComponent', () => {
  let component: KnjizaraComponent;
  let fixture: ComponentFixture<KnjizaraComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [KnjizaraComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(KnjizaraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
