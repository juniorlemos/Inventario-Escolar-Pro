import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AssetMovement } from '../Models/AssetMovement';
import { PagedResult } from '../Models/Pagination';
import { map, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AssetMovementService {
  private readonly baseURL = `${environment.apiURL}assetmovement`;

  constructor(private http: HttpClient) { }

    getAllAssetMovements(
  page: number = 1,
  pageSize: number = 10,
  searchTerm: string = '',
  statusFiltro: boolean | null = null
): Observable<PagedResult<AssetMovement>> {

  let params = new HttpParams()
    .set('page', page.toString())
    .set('pageSize', pageSize.toString());

  if (searchTerm && searchTerm.trim() !== '') {
    params = params.set('searchTerm', searchTerm.trim());
  }

  if (statusFiltro !== null) {
    params = params.set('isCanceled', statusFiltro.toString());
  }

  return this.http.get<PagedResult<AssetMovement>>(this.baseURL, { params }).pipe(take(1));
}

  getAssetMovementById(id: number): Observable<AssetMovement> {
      return this.http.get<AssetMovement>(`${this.baseURL}/${id}`).pipe(take(1));
    }

  insertAssetMovement(assetMovement: AssetMovement): Observable<AssetMovement> {
      return this.http.post<AssetMovement>(this.baseURL, assetMovement).pipe(take(1));
    }

  updateAssetMovement(assetMovement: Partial<AssetMovement>, cancelReason: string): Observable<AssetMovement> {
  const url = `${this.baseURL.replace(/\/$/, '')}/${assetMovement.id}?cancelReason=${encodeURIComponent(cancelReason)}`;

  return this.http.put<AssetMovement>(url, assetMovement).pipe(take(1));
}

}
