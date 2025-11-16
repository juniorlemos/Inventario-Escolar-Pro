import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavHeaderComponent } from '../../../layout/nav-header/nav-header.component';


@Component({
  selector: 'app-assets',
  imports: [RouterModule,NavHeaderComponent],
  templateUrl: './asset.component.html',
  styleUrl: './asset.component.scss'
})
export class AssetsComponent {

}
