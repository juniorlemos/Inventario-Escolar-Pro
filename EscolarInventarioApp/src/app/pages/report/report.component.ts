import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavHeaderComponent } from '../../layout/nav-header/nav-header.component';

@Component({
  selector: 'app-report',
  imports: [RouterModule,NavHeaderComponent],
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss'
})
export class ReportComponent {

}
