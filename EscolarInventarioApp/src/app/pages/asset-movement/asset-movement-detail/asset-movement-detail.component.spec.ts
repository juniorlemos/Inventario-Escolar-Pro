import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetMovementDetailComponent } from './asset-movement-detail.component';

describe('AssetMovementDetailComponent', () => {
  let component: AssetMovementDetailComponent;
  let fixture: ComponentFixture<AssetMovementDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssetMovementDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssetMovementDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
