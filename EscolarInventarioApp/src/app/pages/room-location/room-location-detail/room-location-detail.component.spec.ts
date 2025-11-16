import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomLocationDetailComponent } from './room-location-detail.component';

describe('RoomLocationDetailComponent', () => {
  let component: RoomLocationDetailComponent;
  let fixture: ComponentFixture<RoomLocationDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoomLocationDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoomLocationDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
