import { Routes } from '@angular/router';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { SettingsProfileComponent } from './pages/settings/settings-profile/settings-profile.component';
import { AssetsComponent } from './pages/asset/components/asset.component';
import { AssetListComponent } from './pages/asset/components/asset-list/asset-list.component';
import { AssetDetailComponent } from './pages/asset/components/asset-detail/asset-detail.component';
import { AssetMovementComponent } from './pages/asset-movement/asset-movement.component';
import { AssetMovementListComponent } from './pages/asset-movement/asset-movement-list/asset-movement-list.component';
import { AssetMovementDetailComponent } from './pages/asset-movement/asset-movement-detail/asset-movement-detail.component';
import { AssetMovementDetailCancelComponent } from './pages/asset-movement/asset-movement-detail-cancel/asset-movement-detail-cancel.component';
import { AssetMovementDetailRegisterComponent } from './pages/asset-movement/asset-movement-detail-register/asset-movement-detail-register.component';
import { ReportComponent } from './pages/report/report.component';
import { ReportListComponent } from './pages/report/report-list/report-list.component';
import { ReportViewerComponent } from './pages/report/report-viewer/report-viewer.component';
import { CategoryComponent } from './pages/category/category.component';
import { CategoryListComponent } from './pages/category/category-list/category-list.component';
import { CategoryDetailComponent } from './pages/category/category-detail/category-detail.component';
import { RoomLocationComponent } from './pages/room-location/room-location.component';
import { RoomLocationListComponent } from './pages/room-location/room-location-list/room-location-list.component';
import { RoomLocationDetailComponent } from './pages/room-location/room-location-detail/room-location-detail.component';
import { LoginComponent } from './pages/auth/login/login/login.component';
import { ForgotPasswordComponent } from './pages/auth/forgot-password/forgot-password/forgot-password.component';
import { authGuard } from './core/guard/auth.guard';
import { ForgotPasswordConfirmationComponent } from './pages/auth/forgot-password-confirmation/forgot-password-confirmation.component';
import { ResetPasswordComponent } from './pages/auth/reset-password/reset-password.component';
import { redirectGuard } from './core/guard/redirect.guard';

export const routes: Routes = [
  { path: '', canActivate: [redirectGuard], pathMatch: 'full' },

  { path: 'login', component: LoginComponent, title: 'Inventario360 – Login' },
  { path: 'forgot-password', component: ForgotPasswordComponent, title: 'Inventario360 – Esqueci minha senha' },
  { path: 'forgot-password/confirmation', component: ForgotPasswordConfirmationComponent, title: 'Inventario360 – Confirmação de redefinição' },
  { path: 'reset-password', component: ResetPasswordComponent, title: 'Inventario360 – Redefinir senha' },

  { path: 'dashboard', component: DashboardComponent,canActivate:[authGuard], title: 'Inventario360 – Dashboard' },

  {
    path: 'settings',
    component: SettingsComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: SettingsProfileComponent, title: 'Inventario360 – Perfil' },
      { path: 'profile', component: SettingsProfileComponent, title: 'Inventario360 – Perfil' },
    ]
  },

  {
    path: 'report',
    component: ReportComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: ReportListComponent, title: 'Inventario360 – Relatórios' },
      { path: 'list', component: ReportListComponent, title: 'Inventario360 – Relatórios' },
      { path: 'view', component: ReportViewerComponent, title: 'Inventario360 – Visualizar Relatório' },
    ]
  },

  {
    path: 'category',
    component: CategoryComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: CategoryListComponent, title: 'Inventario360 – Categorias' },
      { path: 'detail', component: CategoryDetailComponent, title: 'Inventario360 – Detalhes da Categoria' },
      { path: 'detail/:id', component: CategoryDetailComponent, title: 'Inventario360 – Detalhes da Categoria' },
      { path: 'list', component: CategoryListComponent, title: 'Inventario360 – Lista de Categorias' },
    ]
  },

  {
    path: 'roomlocation',
    component: RoomLocationComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: RoomLocationListComponent, title: 'Inventario360 – Locais' },
      { path: 'detail', component: RoomLocationDetailComponent, title: 'Inventario360 – Detalhes do Local' },
      { path: 'detail/:id', component: RoomLocationDetailComponent, title: 'Inventario360 – Detalhes do Local' },
      { path: 'list', component: RoomLocationListComponent, title: 'Inventario360 – Lista de Locais' },
    ]
  },

  {
    path: 'asset',
    component: AssetsComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: AssetListComponent, title: 'Inventario360 – Ativos' },
      { path: 'detail', component: AssetDetailComponent, title: 'Inventario360 – Detalhes do Ativo' },
      { path: 'detail/:id', component: AssetDetailComponent, title: 'Inventario360 – Detalhes do Ativo' },
      { path: 'list', component: AssetListComponent, title: 'Inventario360 – Lista de Ativos' },
    ]
  },

  {
    path: 'assetmovement',
    component: AssetMovementComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: AssetMovementListComponent, title: 'Inventario360 – Movimentações' },
      { path: 'detail', component: AssetMovementDetailComponent, title: 'Inventario360 – Detalhes da Movimentação' },
      { path: 'detail/:id', component: AssetMovementDetailComponent, title: 'Inventario360 – Detalhes da Movimentação' },
      { path: 'detail-cancel/:id', component: AssetMovementDetailCancelComponent, title: 'Inventario360 – Cancelar Movimentação' },
      { path: 'detail-cancel', component: AssetMovementDetailCancelComponent, title: 'Inventario360 – Cancelar Movimentação' },
      { path: 'detail-register/:id', component: AssetMovementDetailRegisterComponent, title: 'Inventario360 – Registrar Movimentação' },
      { path: 'detail-register', component: AssetMovementDetailRegisterComponent, title: 'Inventario360 – Registrar Movimentação' },
      { path: 'list', component: AssetMovementListComponent, title: 'Inventario360 – Lista de Movimentações' },
    ]
  },

  { path: '**', redirectTo: 'login' }
];
