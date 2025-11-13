import {AssetMovementService} from '../../../core/Services/asset-movement.service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';
import { AssetMovement } from '../../../core/models/AssetMovement';
import { DateFormatPipe } from '../../../core/Pipe/date-format.pipe';

@Component({
  selector: 'app-asset-movement-detail',
  imports: [ReactiveFormsModule, CommonModule, NgxSpinnerModule,DateFormatPipe],
  templateUrl: './asset-movement-detail.component.html',
  styleUrl: './asset-movement-detail.component.scss'
})
export class AssetMovementDetailComponent implements OnInit {


  assetMovement = {} as AssetMovement;

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private assetMovements: AssetMovementService,
    private toastr: ToastrService,
  ) { }

  ngOnInit(): void {

    const assetMovementIdParam = this.route.snapshot.paramMap.get('id');
    if (assetMovementIdParam) {
      this.getAssetMovementById(+assetMovementIdParam);
    }
  }

  getAssetMovementById(id: number): void {
    this.assetMovements.getAssetMovementById(id).subscribe({
      next: (movement: AssetMovement) => {
        this.assetMovement = movement;
      },
      error: (error) => this.toastr.error('Erro ao carregar movimentação',error)
    });
  }

  goBack(): void {
      this.location.back();
    }

}



