import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavHeaderComponent } from '../../layout/nav-header/nav-header.component';


@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [RouterModule,NavHeaderComponent],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {

}
