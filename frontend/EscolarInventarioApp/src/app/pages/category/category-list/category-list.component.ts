import { CommonModule } from '@angular/common';
import { Component, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ModalModule, BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../../../core/models/Category';
import { CategoryService } from '../../../core/Services/category.service';

@Component({
  selector: 'app-category-list',
  imports: [CommonModule, PaginationModule, FormsModule,RouterModule,TooltipModule,ModalModule],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss'
})
export class CategoryListComponent {
  modalRef?: BsModalRef;


  selectedCategory?: Category;
  data: Category[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  searchTerm: string = '';



  constructor(private categoryService: CategoryService,
              private modalService: BsModalService,
              private router: Router,
              private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(page: number = 1) {
  this.categoryService.getAllCategories(page, this.pageSize, this.searchTerm)
    .subscribe(result => {

      if (!result) {
        this.data = [];
        this.totalItems = 0;
        return;
      }

      if (result.totalCount === 0 && this.searchTerm) {
        this.searchTerm = '';
        this.loadCategories(this.currentPage);
        return;
      }

      this.data = result.items ?? [];
      this.totalItems = result.totalCount ?? 0;
      this.currentPage = result.page ?? page;
    });
}


  public pageChanged(event: { page: number }): void {
    this.currentPage = event.page;
    this.loadCategories(this.currentPage);
  }

onSearchChange(event: any) {
  this.searchTerm = event.target.value;
  this.currentPage = 1;
  this.loadCategories(this.currentPage);
}


  openModal(event: any, template: TemplateRef<any>, category: Category) : void {
    event.stopPropagation();
    this.selectedCategory = category;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }


  detalheCategory(id: number): void {
    this.router.navigate([`category/detail/${id}`]);
  }

  decline(): void {
    this.modalRef?.hide();
  }

  confirm() {
  this.modalRef?.hide();
  if (!this.selectedCategory) return;

  this.categoryService.deleteCategory(this.selectedCategory.id)
    .subscribe({
      next: () => {
        this.toastr.success(
          `A categoria "${this.selectedCategory?.name}" foi deletada com sucesso.`,
          'Deletado!'
        );
        this.currentPage = 1;
        this.loadCategories(this.currentPage);      },
      error: (error) => {
        console.error(error);
        this.toastr.error(
          `Erro ao tentar deletar o item "${this.selectedCategory?.name}"`,
          'Erro'
        );
      }
    });
}

  }
