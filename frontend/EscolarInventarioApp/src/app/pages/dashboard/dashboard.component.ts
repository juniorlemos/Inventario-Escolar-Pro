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
      icone: 'fal fa-box-open',
      cor: 'primary',
      rota: '/asset'
    },
    movimentacoes: {
      titulo: 'Movimentações',
      valor: 0,
      icone: 'fal fa-exchange',
      cor: 'success',
      rota: '/assetmovement'
    },
    localizacoes: {
      titulo: 'Loalizações',
      valor: 0,
      icone: 'fal fa-map-marker',
      cor: 'success',
      rota: '/roomlocation'
    },
    categorias: {
      titulo: 'Categorias',
      valor: 0,
      icone: 'fal fa-tags',
      cor: 'success',
      rota: '/category'
    },
    relatorios: {
      titulo: 'Relatórios',
      valor: 0,
      icone: 'fal fa-file-alt',
      cor: 'warning',
      rota: '/report'
    },
    perfil: {
      titulo: 'Perfil',
      valor: 0,
      icone: 'fal fa-user',
      cor: 'info',
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
    this.router.navigate(['/login'], { replaceUrl: true });
  }
}
