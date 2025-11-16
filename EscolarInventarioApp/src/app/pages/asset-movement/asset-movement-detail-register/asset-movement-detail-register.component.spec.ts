import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetMovementDetailRegisterComponent } from './asset-movement-detail-register.component';

describe('AssetMovementDetailRegisterComponent', () => {
  let component: AssetMovementDetailRegisterComponent;
  let fixture: ComponentFixture<AssetMovementDetailRegisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssetMovementDetailRegisterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssetMovementDetailRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
