import { Component, Input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NavSideItemsService } from '../../core/Services/nav-side-items.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav-header',
  imports: [CommonModule,RouterModule],
  templateUrl: './nav-header.component.html',
  styleUrl: './nav-header.component.scss'
})
export class NavHeaderComponent {
@Input() titulo: string = '';

  headerItems: any[] = [];

  constructor(private navSideItemsService: NavSideItemsService) {}

  ngOnInit(): void {
    this.headerItems = this.navSideItemsService.getItems()
      .filter(item => item.showInHeader);
  }
}
