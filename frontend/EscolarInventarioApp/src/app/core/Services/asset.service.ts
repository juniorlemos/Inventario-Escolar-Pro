import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { take, map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { Asset } from '../models/Asset';
import { PagedResult } from '../models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class AssetService {
  private readonly baseURL = `${environment.apiURL}asset`;

  constructor(private http: HttpClient) {}

  getAllAssets(
  page: number = 1,
  pageSize: number = 10,
  searchTerm: string = '',
  conservationState?: number
): Observable<PagedResult<Asset>> {
  let params = new HttpParams()
    .set('page', page.toString())
    .set('pageSize', pageSize.toString());

  if (searchTerm && searchTerm.trim() !== '') {
    params = params.set('searchTerm', searchTerm.trim());
  }

  if (conservationState != null) {
    params = params.set('conservationState', conservationState.toString());
  }

  return this.http.get<PagedResult<Asset>>(this.baseURL, { params }).pipe(take(1));
}

  insertAsset(asset: Asset): Observable<Asset> {
    return this.http.post<Asset>(this.baseURL, asset).pipe(take(1));
  }

  updateAsset(asset: Asset): Observable<Asset> {
    const url = `${this.baseURL.replace(/\/$/, '')}/${asset.id}`;

    return this.http.put<void>(url, asset).pipe(
      take(1),
      map(() => asset)
    );
  }

  getAssetById(id: number): Observable<Asset> {
    return this.http.get<Asset>(`${this.baseURL}/${id}`).pipe(take(1));
  }

  deleteAsset(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseURL}/${id}`).pipe(take(1));
  }
}
