import { CommonModule } from '@angular/common';
import { Component, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ToastrService } from 'ngx-toastr';
import { AssetMovement } from '../../../core/models/AssetMovement';
import { AssetMovementService } from '../../../core/Services/asset-movement.service';
import { DateFormatPipe } from '../../../core/Pipe/date-format.pipe';

@Component({
  selector: 'app-asset-movement-list',
  imports: [CommonModule, PaginationModule, FormsModule,RouterModule,TooltipModule,DateFormatPipe],
  templateUrl: './asset-movement-list.component.html',
  styleUrl: './asset-movement-list.component.scss'
})
export class AssetMovementListComponent {


selectedAssetMovement?: AssetMovement;
  data: AssetMovement[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  searchTerm: string = '';
  statusFiltro: boolean | null = null;


  constructor(private assetMovementService: AssetMovementService,
              private router: Router,
              private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadAssetMovements();
  }

  loadAssetMovements(page: number = 1) {
  this.assetMovementService.getAllAssetMovements(page, this.pageSize, this.searchTerm,this.statusFiltro)
    .subscribe(result => {

      if (!result) {
        this.data = [];
        this.totalItems = 0;
        return;
      }

      if (result.totalCount === 0 && this.searchTerm) {
        this.searchTerm = '';
        this.loadAssetMovements(this.currentPage);
        return;
      }

      this.data = result.items ?? [];
      this.totalItems = result.totalCount ?? 0;
      this.currentPage = result.page ?? page;
    });
}

  public pageChanged(event: { page: number }): void {
    this.currentPage = event.page;
    this.loadAssetMovements(this.currentPage);
  }

filtrarPorStatus() {
  this.currentPage = 1;
  this.loadAssetMovements();
}
onSearchChange(event: any) {
  this.searchTerm = event.target.value;
  this.currentPage = 1;
  this.loadAssetMovements(this.currentPage);}


  detalheAsset(id: number): void {
    this.router.navigate([`assetmovement/detail/${id}`]);
  }

  informarMovimentacaoCancelada() {
    this.toastr.info('Esta movimentação já foi cancelada.', 'Informação');
  }
cancelarMovimentacao(id: number) {
  this.router.navigate([ `assetmovement/detail-cancel/${id}`])
  }
}

