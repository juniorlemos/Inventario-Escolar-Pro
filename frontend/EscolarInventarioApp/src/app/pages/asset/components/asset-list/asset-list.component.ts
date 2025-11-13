import { CommonModule } from '@angular/common';
import { Asset } from '../../../../core/models/Asset';
import { AssetService } from './../../../../core/Services/asset.service';
import { Component, TemplateRef } from '@angular/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule, BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../../../../core/models/Category';
import { ConservationState } from '../../../../core/models/enum/ConservationState.enum';
import { RoomLocation } from '../../../../core/models/RoomLocation';


@Component({
  selector: 'app-asset-list',
  imports: [CommonModule, PaginationModule, FormsModule,RouterModule,TooltipModule,ModalModule],
  templateUrl: './asset-list.component.html',
  standalone: true,
  styleUrl: './asset-list.component.scss'

})

export class AssetListComponent {

  modalRef?: BsModalRef;


selectedAsset?: Asset;
  data: Asset[] = [];
  roomlocations: RoomLocation[] = [];
  categories: Category[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  searchTerm: string = '';
  selectedConservation?: number;


  constructor(private assetService: AssetService,
              private modalService: BsModalService,
              private router: Router,
              private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadAssets();
  }

loadAssets(page: number = 1) {
  this.assetService.getAllAssets(page, this.pageSize, this.searchTerm, this.selectedConservation)
    .subscribe(result => {
      if (!result) {
        this.data = [];
        this.totalItems = 0;
        return;
      }

      if (result.totalCount === 0 && this.searchTerm) {
        this.searchTerm = '';
        this.loadAssets(page);
        return;
      }

      this.data = result.items ?? [];
      this.totalItems = result.totalCount ?? 0;
      this.currentPage = result.page ?? page;
    });
}
get conservationStates(): { id: number; name: string }[] {
  return Object.entries(ConservationState)
    .filter(([key, value]) => !isNaN(Number(value)))
    .map(([key, value]) => ({
      id: value as number,
      name: key
    }));
  }

  getConservationStateLabel(value: string | number): string {
    if (typeof value === 'string') {
    return value.toUpperCase();
  }

  const numericValue = Number(value);
  const entry = Object.entries(ConservationState)
    .find(([key, val]) => val === numericValue);
  return entry ? entry[0] : 'DESCONHECIDO';
}

onFilterChange() {
  this.currentPage = 1;
  this.loadAssets(this.currentPage);
  }

  public pageChanged(event: { page: number }): void {
    this.currentPage = event.page;
    this.loadAssets(this.currentPage);
  }

onSearchChange(event: any) {
  this.searchTerm = event.target.value;
  this.currentPage = 1;
  this.loadAssets(this.currentPage);}

  openModal(event: any, template: TemplateRef<any>, asset: Asset) : void {
    event.stopPropagation();
    this.selectedAsset = asset;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  detalheAsset(id: number): void {
    this.router.navigate([`asset/detail/${id}`]);
  }

  decline(): void {
    this.modalRef?.hide();
  }

  confirm() {
  this.modalRef?.hide();
  if (!this.selectedAsset) return;

  this.assetService.deleteAsset(this.selectedAsset.id)
    .subscribe({
      next: () => {
        this.toastr.success(
          `O item "${this.selectedAsset?.name}" foi deletado com sucesso.`,
          'Deletado!'
        );
        this.loadAssets();
      },
      error: (error) => {
        this.toastr.error(
          `Erro ao tentar deletar o item "${this.selectedAsset?.name}"`,
          'Erro'
        );
      }
    });
}
  }
