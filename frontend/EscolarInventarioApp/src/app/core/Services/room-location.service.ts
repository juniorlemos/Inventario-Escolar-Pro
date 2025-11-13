import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/Pagination';
import { RoomLocation } from '../models/RoomLocation';

@Injectable({
  providedIn: 'root'
})
export class RoomLocationService {

baseURL = environment.apiURL + 'roomLocation';

constructor(private http: HttpClient) { }

  public getAllRoomLocations(page: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<PagedResult<RoomLocation>> {
    let params = new HttpParams()
          .set('page', page.toString())
          .set('pageSize', pageSize.toString());

        if (searchTerm && searchTerm.trim() !== '') {
        params = params.set('searchTerm', searchTerm.trim());
      }

  return this.http.get<PagedResult<RoomLocation>>(`${this.baseURL}`, { params }).pipe(take(1));
  }

    public getRoomLocationById(id: number): Observable<RoomLocation> {
      return this.http
        .get<RoomLocation>(`${this.baseURL}/${id}`)
        .pipe(take(1));
    }

  public insertRoomLocation(evento: RoomLocation): Observable<RoomLocation> {
    return this.http
      .post<RoomLocation>(this.baseURL, evento)
      .pipe(take(1));
  }

   public updateRoomLocation(evento: RoomLocation): Observable<RoomLocation> {
      return this.http
        .put<RoomLocation>(`${this.baseURL}/${evento.id}`, evento)
        .pipe(take(1));
    }

  public deleteRoomLocation(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

}
