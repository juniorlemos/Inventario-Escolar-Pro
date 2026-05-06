import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { SchoolService } from '../../core/Services/school.service';
import { DashboardData } from '../../core/models/DashboardData';
import { School } from '../../core/models/School';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  school?: School;

  dashboardData: DashboardData = {
  inventario: {
    titulo: 'Inventário',
    valor: 0,
    icone: 'fa-solid fa-boxes-stacked',
    cor: 'primary',
    rota: '/asset'
  },
  movimentacoes: {
    titulo: 'Movimentações',
    valor: 0,
    icone: 'fa-solid fa-arrow-right-arrow-left',
    cor: 'success',
    rota: '/assetmovement'
  },
  localizacoes: {
    titulo: 'Localizações',
    valor: 0,
    icone: 'fa-solid fa-location-dot',
    cor: 'info',
    rota: '/roomlocation'
  },
  categorias: {
    titulo: 'Categorias',
    valor: 0,
    icone: 'fa-solid fa-layer-group',
    cor: 'secondary',
    rota: '/category'
  },
  relatorios: {
    titulo: 'Relatórios',
    valor: 0,
    icone: 'fa-solid fa-chart-column',
    cor: 'warning',
    rota: '/report'
  },
  perfil: {
    titulo: 'Perfil',
    valor: 0,
    icone: 'fa-solid fa-circle-user',
    cor: 'dark',
    rota: '/settings'
  }
};
  constructor(
    private router: Router,
    private schoolService: SchoolService,
    private toast: ToastrService

  ) {}

  ngOnInit(): void {
    this.loadSchool();
  }

  private loadSchool(): void {
    this.schoolService.getSchoolData().subscribe({
      next: (school) => {
        this.school = school;
      },
      error: (err) => {
        this.toast.error('Erro ao carregar dados da escola. Faça login novamente.', 'Erro');
        this.logout();
      }
    });
  }


  logout(): void {
    localStorage.clear();
    sessionStorage.clear();
    this.router.navigate(['/login'], { replaceUrl: true }).then(() => {
    window.location.replace('/login');
  });
  }
}
