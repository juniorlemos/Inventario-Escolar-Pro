import { Injectable } from '@angular/core';

export const NAV_ITEMS = [
  { routeLink: '/dashboard', icon: 'fal fa-home', label: 'Inicio', showInHeader: false },
  { routeLink: '/asset', icon: 'fal fa-box-open', label: 'Inventario', showInHeader: true },
  { routeLink: '/roomlocation', icon: 'fal fa-map-marker', label: 'Localizações', showInHeader: false },
  { routeLink: '/category', icon: 'fal fa-tags', label: 'Categorias', showInHeader: false },
  { routeLink: '/assetmovement', icon: 'fal fa-exchange', label: 'Movimentações', showInHeader: true },
  { routeLink: '/report', icon: 'fal fa-file', label: 'Relatórios', showInHeader: true },
  { routeLink: '/settings', icon: 'fal fa-user', label: 'Perfil', showInHeader: false },
  { action: 'logout', icon: 'fal fa-sign-out', label: 'Sair', showInHeader: false },
];

@Injectable({
  providedIn: 'root'
})
export class NavSideItemsService {

  constructor() { }

  getItems() {

    return NAV_ITEMS;
  }
}
