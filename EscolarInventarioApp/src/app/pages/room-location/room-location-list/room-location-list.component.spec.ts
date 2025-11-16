import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomLocationListComponent } from './room-location-list.component';

describe('RoomLocationListComponent', () => {
  let component: RoomLocationListComponent;
  let fixture: ComponentFixture<RoomLocationListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoomLocationListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoomLocationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
