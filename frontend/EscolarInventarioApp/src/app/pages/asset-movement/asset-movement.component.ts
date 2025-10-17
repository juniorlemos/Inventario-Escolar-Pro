import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavHeaderComponent } from '../../layout/nav-header/nav-header.component';

@Component({
  selector: 'app-asset-movement',
  imports: [RouterModule,NavHeaderComponent],
  templateUrl: './asset-movement.component.html',
  styleUrl: './asset-movement.component.scss'
})
export class AssetMovementComponent {

}
