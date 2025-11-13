import { CommonModule } from '@angular/common';
import { Component, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ModalModule, BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ToastrService } from 'ngx-toastr';
import { RoomLocationService } from '../../../core/Services/room-location.service';
import { RoomLocation } from '../../../core/models/RoomLocation';

@Component({
  selector: 'app-room-location-list',
  imports: [CommonModule, PaginationModule, FormsModule,RouterModule,TooltipModule,ModalModule],
  templateUrl: './room-location-list.component.html',
  styleUrl: './room-location-list.component.scss'
})
export class RoomLocationListComponent {

  modalRef?: BsModalRef;

  selectedRoomLocation?: RoomLocation;
  data: RoomLocation[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  searchTerm: string = '';



  constructor(private roomLocationService: RoomLocationService,
              private modalService: BsModalService,
              private router: Router,
              private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadRoomLocations();
  }

  loadRoomLocations(page: number = 1) {
  this.roomLocationService.getAllRoomLocations(page, this.pageSize, this.searchTerm)
    .subscribe(result => {
      if (!result) {
        this.data = [];
        this.totalItems = 0;
        return;
      }

      if (result.totalCount === 0 && this.searchTerm) {
        this.searchTerm = '';
        this.loadRoomLocations(this.currentPage);
        return;
      }

      this.data = result.items ?? [];
      this.totalItems = result.totalCount ?? 0;
      this.currentPage = result.page ?? page;
    });
}



  public pageChanged(event: { page: number }): void {
    this.currentPage = event.page;
    this.loadRoomLocations(this.currentPage);
  }

onSearchChange(event: any) {
  this.searchTerm = event.target.value;
  this.currentPage = 1;
  this.loadRoomLocations(this.currentPage);
}


  openModal(event: any, template: TemplateRef<any>, roomLocation: RoomLocation) : void {
    event.stopPropagation();
    this.selectedRoomLocation = roomLocation;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }


  detalheCategory(id: number): void {
    this.router.navigate([`roomlocation/detail/${id}`]);
  }

  decline(): void {
    this.modalRef?.hide();
  }

  confirm() {
  this.modalRef?.hide();
  if (!this.selectedRoomLocation) return;

  this.roomLocationService.deleteRoomLocation(this.selectedRoomLocation.id)
    .subscribe({
      next: () => {
        this.toastr.success(
          `A localização "${this.selectedRoomLocation?.name}" foi deletada com sucesso.`,
          'Deletado!'
        );
        this.currentPage = 1;
        this.loadRoomLocations(this.currentPage);
      },
      error: (error) => {
        this.toastr.error(
          `Erro ao tentar deletar o item "${this.selectedRoomLocation?.name}"`,
          'Erro'
        );
      }
    });
  }
}


