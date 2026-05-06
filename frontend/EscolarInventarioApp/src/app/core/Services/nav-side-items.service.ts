import { Injectable } from '@angular/core';

export const NAV_ITEMS = [
  { routeLink: '/dashboard', icon: 'fa-solid fa-home', label: 'Inicio', showInHeader: false },
  { routeLink: '/asset', icon: 'fa-solid fa-box-open', label: 'Inventario', showInHeader: true },
  { routeLink: '/roomlocation', icon: 'fa-solid fa-location-dot', label: 'Localizações', showInHeader: false },
  { routeLink: '/category', icon: 'fa-solid fa-tags', label: 'Categorias', showInHeader: false },
  { routeLink: '/assetmovement', icon: 'fa-solid fa-right-left', label: 'Movimentações', showInHeader: true },
  { routeLink: '/report', icon: 'fa-solid fa-file', label: 'Relatórios', showInHeader: true },
  { routeLink: '/settings', icon: 'fa-solid fa-user', label: 'Perfil', showInHeader: false },
  { action: 'logout', icon: 'fa-solid fa-right-from-bracket', label: 'Sair', showInHeader: false },
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
