import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetMovementDetailCancelComponent } from './asset-movement-detail-cancel.component';

describe('AssetMovementDetailCancelComponent', () => {
  let component: AssetMovementDetailCancelComponent;
  let fixture: ComponentFixture<AssetMovementDetailCancelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssetMovementDetailCancelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssetMovementDetailCancelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
