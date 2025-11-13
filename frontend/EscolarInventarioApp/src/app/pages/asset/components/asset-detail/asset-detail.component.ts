import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule, Location } from '@angular/common';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { Asset } from '../../../../core/models/Asset';
import { AssetService } from '../../../../core/Services/asset.service';
import { CategoryService } from '../../../../core/Services/category.service';
import { RoomLocationService } from '../../../../core/Services/room-location.service';
import { PagedResult } from '../../../../core/models/Pagination';
import { ConservationState } from '../../../../core/models/enum/ConservationState.enum';
import { RoomLocation } from '../../../../core/models/RoomLocation';
import { Category } from '../../../../core/models/Category';
import { NgxMaskDirective } from 'ngx-mask';


@Component({
  selector: 'app-asset-detail',
  imports: [ReactiveFormsModule, CommonModule, NgxSpinnerModule,NgxMaskDirective],
  templateUrl: './asset-detail.component.html',
  styleUrls: ['./asset-detail.component.scss']
})
export class AssetDetailComponent implements OnInit {
  form!: FormGroup;
  asset = {} as Asset;
  categories: Category[] = [];
  roomlocations: RoomLocation[] = [];
  isEditMode = false;

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private assetService: AssetService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private roomLocationService: RoomLocationService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.validation();
    this.getCategories();
    this.getRoomLocations();

    const assetIdParam = this.route.snapshot.paramMap.get('id');
    if (assetIdParam) {
      this.isEditMode = true;
      this.getAssetById(+assetIdParam);
    }
  }

  validation(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', Validators.maxLength(200)],
      patrimonyCode: ['', Validators.pattern('^[1-9]\\d*$')],
      acquisitionValue: ['', [this.validateCurrency]],
      roomLocationId: [''],
      categoryId: ['', Validators.required],
      conservationState: ['', Validators.required],
      serieNumber: ['', Validators.maxLength(30)],
    });
  }

  saveAsset(): void {
    if (this.form.invalid) return;

    const assetData = {
      ...this.form.value,
     acquisitionValue: Number(
    this.form.value.acquisitionValue
      .replace(/[R$\.\s]/g, '')
      .replace(',', '.')
  ),
      id: this.isEditMode ? this.asset.id : undefined
    };

    const request = this.isEditMode
      ? this.assetService.updateAsset(assetData)
      : this.assetService.insertAsset(assetData);

    request.subscribe({
      next: () => {
        this.toastr.success('Item salvo com sucesso!', 'Sucesso');
        this.location.back();
      },
      error: () => this.toastr.error('Erro ao salvar item', 'Erro')
    });
  }

  getAssetById(id: number): void {
    this.assetService.getAssetById(id).subscribe({
      next: (asset: Asset) => {
        this.asset = asset;

        this.form.patchValue({
          name: asset.name,
          description: asset.description,
          patrimonyCode: asset.patrimonyCode,
          acquisitionValue: asset.acquisitionValue,
          roomLocationId: asset.roomLocation?.id ?? '',
          categoryId: asset.category?.id ?? '',
          conservationState: this.mapConservationState(asset.conservationState),
          serieNumber: asset.serieNumber
        });
      },
      error: () => this.toastr.error('Erro ao carregar o item.', 'Erro')
    });
  }

validateCurrency(control: FormControl) {
  if (!control.value) return null;

  const value = typeof control.value === 'string'
    ? control.value.replace(/[R$\.\s]/g, '').replace(',', '.')
    : control.value;

  const numericValue = Number(value);
  return isNaN(numericValue) || numericValue <= 0 ? { invalidCurrency: true } : null;
}

  mapConservationState(value: string | number): number | null {
    if (typeof value === 'number') return value;

    const entry = Object.entries(ConservationState).find(
      ([key]) => key.toUpperCase() === value.toUpperCase()
    );
    return entry ? (entry[1] as number) : null;
  }

  get conservationStates(): { id: number; name: string }[] {
    return Object.entries(ConservationState)
      .filter(([key, value]) => !isNaN(Number(value)))
      .map(([key, value]) => ({
        id: value as number,
        name: key
      }));
  }

  getCategories(): void {
    this.categoryService.getAllCategories(1, 0).subscribe({
      next: (pagedResult: PagedResult<{ id: number; name: string }>) => {
        this.categories = pagedResult.items;
      }
    });
  }

  getRoomLocations(): void {
    this.roomLocationService.getAllRoomLocations(1, 0).subscribe({
      next: (pagedResult: PagedResult<{ id: number; name: string }>) => {
        this.roomlocations = pagedResult.items;
      }
    });
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
