import { CommonModule, Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Asset } from '../../../core/Models/Asset';
import { Category } from '../../../core/Models/Category';
import { PagedResult } from '../../../core/Models/Pagination';
import { RoomLocation } from '../../../core/Models/RoomLocation';
import { AssetSummary } from '../../../core/Models/AssetSummary';
import { AssetMovement } from '../../../core/Models/AssetMovement';

import { AssetService } from '../../../core/Services/asset.service';
import { CategoryService } from '../../../core/Services/category.service';
import { RoomLocationService } from '../../../core/Services/room-location.service';
import { AssetMovementService } from '../../../core/Services/asset-movement.service';

@Component({
  selector: 'app-asset-movement-detail-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, NgxSpinnerModule],
  templateUrl: './asset-movement-detail-register.component.html',
  styleUrls: ['./asset-movement-detail-register.component.scss']
})
export class AssetMovementDetailRegisterComponent implements OnInit {

  form!: FormGroup;
  asset: Asset = {} as Asset;
  categories: Category[] = [];
  assets: AssetSummary[] = [];
  roomlocations: RoomLocation[] = [];

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private categoryService: CategoryService,
    private roomLocationService: RoomLocationService,
    private assetMovementService: AssetMovementService,
    private assetService: AssetService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.validation();
    this.getCategories();
    this.getRoomLocations();
    this.getAssets();
  }

  validation(): void {
    this.form = this.fb.group({
      responsible: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      assetId: ['', Validators.required],
      toRoomId: ['', Validators.required],
      fromRoomId: [{ value: '', disabled: true }, Validators.required]
    });
  }

  saveAssetMovement(): void {
    if (this.form.invalid) return;

    const assetData: AssetMovement = this.form.getRawValue();

    this.assetMovementService.insertAssetMovement(assetData).subscribe({
      next: () => {
        this.toastr.success('Item salvo com sucesso!', 'Sucesso');
        this.location.back();
      },
      error: (error) => {
        this.toastr.error('Erro ao salvar item', error);
      }
    });
  }

  getCategories(): void {
    this.categoryService.getAllCategories(1, 0).subscribe({
      next: (pagedResult: PagedResult<{ id: number; name: string }>) => {
        this.categories = pagedResult.items;
      }
    });
  }

  getAssets(): void {
    this.assetService.getAllAssets(1, 0).subscribe({
      next: (pagedResult: PagedResult<AssetSummary>) => {
        this.assets = pagedResult.items;
      }
    });
  }

  getRoomLocations(): void {
    this.roomLocationService.getAllRoomLocations(1, 0).subscribe({
      next: (pagedResult: PagedResult<RoomLocation>) => {
        this.roomlocations = pagedResult.items;
      }
    });
  }

  getRoomNameById(id: number | null | undefined): string {
    if (!id) return '';
    const room = this.roomlocations.find(loc => loc.id === id);
    return room ? room.name : '';
  }

  onAssetSelected(event: any): void {
    const assetId = +event.target.value;
    const selectedAsset = this.assets.find(a => a.id === assetId);

    if (selectedAsset?.roomLocation?.id) {
      this.form.patchValue({
        fromRoomId: selectedAsset.roomLocation.id
      });
    } else {
      this.form.patchValue({ fromRoomId: null });
    }
  }

  resetForm(): void {
    this.form.reset();
  }

  goBack(): void {
    this.location.back();
  }

  cssValidator(campoForm: FormControl | AbstractControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

}
