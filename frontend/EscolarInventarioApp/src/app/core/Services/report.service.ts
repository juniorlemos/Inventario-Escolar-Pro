import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

baseURL = environment.apiURL + 'report';

    constructor(private http: HttpClient) {}

  getInventoryReport(): Observable<Blob> {
    return this.http.get(`${this.baseURL}/inventory`, { responseType: 'blob' });
  }

  getAssetsByLocationReport(): Observable<Blob> {
    return this.http.get(`${this.baseURL}/by-location`, { responseType: 'blob' });
  }

  getAssetMovementsReport(): Observable<Blob> {
    return this.http.get(`${this.baseURL}/movements`, { responseType: 'blob' });
  }

  getAssetCanceledMovementsReport(): Observable<Blob> {
    return this.http.get(`${this.baseURL}/canceled-movements`, { responseType: 'blob' });
  }

  getAssetsByConservationReport(): Observable<Blob> {
    return this.http.get(`${this.baseURL}/by-conservation`, { responseType: 'blob' });
  }

  getAssetsByCategoryReport(): Observable<Blob> {
    return this.http.get(`${this.baseURL}/by-category`, { responseType: 'blob' });
  }
}
