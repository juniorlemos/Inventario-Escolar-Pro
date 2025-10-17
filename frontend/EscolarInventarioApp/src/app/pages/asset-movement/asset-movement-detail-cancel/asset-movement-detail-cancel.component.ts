import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';
import { AssetMovement } from '../../../core/Models/AssetMovement';
import { AssetMovementService } from '../../../core/Services/asset-movement.service';

@Component({
  selector: 'app-asset-movement-detail-cancel',
  imports: [ReactiveFormsModule, CommonModule, NgxSpinnerModule],
  templateUrl: './asset-movement-detail-cancel.component.html',
  styleUrl: './asset-movement-detail-cancel.component.scss'
})
export class AssetMovementDetailCancelComponent implements OnInit {

form!: FormGroup;
  assetMovement = {} as AssetMovement;

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private assetMovements: AssetMovementService
  ) {}

  ngOnInit(): void {
    this.validation();

    const assetMovementIdParam = this.route.snapshot.paramMap.get('id');
    if (assetMovementIdParam) {
      this.getAssetMovementById(+assetMovementIdParam);
    }
  }

  validation(): void {
    this.form = this.fb.group({
      cancelReason: ['', Validators.maxLength(200)]
    });
  }

  UpdateAssetMovement(): void {
  if (this.form.invalid) return;

  const updateData: Partial<AssetMovement> = {
    id: this.assetMovement.id
  };

  const cancelReason: string = this.form.value.cancelReason;

  this.assetMovements.updateAssetMovement(updateData, cancelReason).subscribe({
    next: () => {
      this.toastr.success('Movimentação cancelada com sucesso!', 'Sucesso');
      this.location.back();
    },
    error: (error) => this.toastr.error('Erro ao cancelar movimentação', "Erro")
  });
}

  getAssetMovementById(id: number): void {
    this.assetMovements.getAssetMovementById(id).subscribe({
      next: (movement: AssetMovement) => {
        this.assetMovement = movement;
      },
      error: (error) => this.toastr.error('Erro ao carregar movimentação', "Erro")
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
