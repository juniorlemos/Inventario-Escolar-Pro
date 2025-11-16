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
import { authGuard } from './core/Gaurd/auth.guard';
import { ForgotPasswordConfirmationComponent } from './pages/auth/forgot-password-confirmation/forgot-password-confirmation.component';
import { ResetPasswordComponent } from './pages/auth/reset-password/reset-password.component';



export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'forgot-password/confirmation', component: ForgotPasswordConfirmationComponent },

 { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },

  {
    path: 'settings',
    component: SettingsComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: SettingsProfileComponent },
      { path: 'profile', component: SettingsProfileComponent },
    ]
  },

  {
    path: 'report',
    component: ReportComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: ReportListComponent },
      { path: 'list', component: ReportListComponent },
      { path: 'view', component: ReportViewerComponent },
    ]
  },

  {
    path: 'category',
    component: CategoryComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: CategoryListComponent },
      { path: 'detail', component: CategoryDetailComponent },
      { path: 'detail/:id', component: CategoryDetailComponent },
      { path: 'list', component: CategoryListComponent },
    ]
  },

  {
    path: 'roomlocation',
    component: RoomLocationComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: RoomLocationListComponent },
      { path: 'detail', component: RoomLocationDetailComponent },
      { path: 'detail/:id', component: RoomLocationDetailComponent },
      { path: 'list', component: RoomLocationListComponent },
    ]
  },

  {
    path: 'asset',
    component: AssetsComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: AssetListComponent },
      { path: 'detail', component: AssetDetailComponent },
      { path: 'detail/:id', component: AssetDetailComponent },
      { path: 'list', component: AssetListComponent },
    ]
  },

  {
    path: 'assetmovement',
    component: AssetMovementComponent,
    canActivate: [authGuard],
    children:[
      { path: '', component: AssetMovementListComponent },
      { path: 'detail', component: AssetMovementDetailComponent },
      { path: 'detail/:id', component: AssetMovementDetailComponent },
      { path: 'detail-cancel/:id', component: AssetMovementDetailCancelComponent },
      { path: 'detail-cancel', component: AssetMovementDetailCancelComponent },
      { path: 'detail-register/:id', component: AssetMovementDetailRegisterComponent },
      { path: 'detail-register', component: AssetMovementDetailRegisterComponent },
      { path: 'list', component: AssetMovementListComponent },
    ]
  },

  { path: '**', redirectTo: 'login' }
];
