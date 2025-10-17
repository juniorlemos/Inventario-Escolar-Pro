import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavHeaderComponent } from '../../layout/nav-header/nav-header.component';

@Component({
  selector: 'app-room-location',
  imports: [RouterModule,NavHeaderComponent],
  templateUrl: './room-location.component.html',
  styleUrl: './room-location.component.scss'
})
export class RoomLocationComponent {

}
