import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavHeaderComponent } from '../../layout/nav-header/nav-header.component';

@Component({
  selector: 'app-category',
  imports: [RouterModule,NavHeaderComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.scss'
})
export class CategoryComponent {

}
