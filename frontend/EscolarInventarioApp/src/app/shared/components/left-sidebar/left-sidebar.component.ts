import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, TemplateRef, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { BsModalRef, BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { NavSideItemsService } from '../../../core/Services/nav-side-items.service';
import { NavItem } from '../../../core/Models/NavItem';
import { AuthService } from '../../../core/Services/auth.service';
import { AuthStore } from '../../../core/stores/auth.store';

@Component({
  selector: 'app-left-sidebar',
  standalone: true,
  imports: [RouterModule, CommonModule, ModalModule],
  templateUrl: './left-sidebar.component.html',
  styleUrls: ['./left-sidebar.component.scss']
})
export class LeftSidebarComponent {
  @Input() isLeftSidebarCollapsed!: boolean;
  @Output() changeIsLeftSidebarCollapsed = new EventEmitter<boolean>();
  items: NavItem[] = [];
  modalRef?: BsModalRef;

  private authService = inject(AuthService);
  private authStore = inject(AuthStore);
  private router = inject(Router);
  private modalService = inject(BsModalService);
  private navSideItemsService = inject(NavSideItemsService);

  ngOnInit(): void {
    this.items = this.navSideItemsService.getItems();
  }

  toggleCollapse(): void {
    this.changeIsLeftSidebarCollapsed.emit(!this.isLeftSidebarCollapsed);
  }

  closeSidenav(): void {
    this.changeIsLeftSidebarCollapsed.emit(true);
  }

  openLogoutModal(event: MouseEvent, template: TemplateRef<any>): void {
    event.preventDefault();
    event.stopPropagation();
    this.modalRef = this.modalService.show(template, { class: 'modal-md' });
  }

  decline(): void {
    this.modalRef?.hide();
  }

  confirmLogout(): void {
    localStorage.clear();
    sessionStorage.clear();
    this.modalRef?.hide();
    this.router.navigate(['/login']);
  }

  get shouldShowSidebar(): boolean {
    const isLoggedIn = this.authStore.isLoggedIn();
    const publicRoutes = ['/login', '/forgot-password'];
    const isPublicRoute = publicRoutes.includes(this.router.url);
    return isLoggedIn && !isPublicRoute;
  }
}
