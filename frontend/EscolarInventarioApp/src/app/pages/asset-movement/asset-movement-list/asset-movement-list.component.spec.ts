import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetMovementListComponent } from './asset-movement-list.component';

describe('AssetMovementListComponent', () => {
  let component: AssetMovementListComponent;
  let fixture: ComponentFixture<AssetMovementListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssetMovementListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssetMovementListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
