import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { UpdateSchoolRequest } from '../Models/UpdateSchoolRequest';
import { School } from '../Models/School';

@Injectable({
  providedIn: 'root'
})
export class SchoolService {

    private baseURL = environment.apiURL + 'school';

constructor(private http: HttpClient) {}

  public getSchoolData(): Observable<School> {
    return this.http.get<School>(`${this.baseURL}/schoolData`).pipe(take(1));
  }

  public getSchoolById(id: number): Observable<School> {
        return this.http
          .get<School>(`${this.baseURL}/${id}`)
          .pipe(take(1));
      }

   updateSchool(id: number, request: UpdateSchoolRequest): Observable<void> {
    return this.http.put<void>(`${this.baseURL}/${id}`, request);
  }
}

