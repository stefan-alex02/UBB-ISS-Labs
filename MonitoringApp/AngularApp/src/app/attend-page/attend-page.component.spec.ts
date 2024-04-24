import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttendPageComponent } from './attend-page.component';

describe('AttendPageComponent', () => {
  let component: AttendPageComponent;
  let fixture: ComponentFixture<AttendPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AttendPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AttendPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
