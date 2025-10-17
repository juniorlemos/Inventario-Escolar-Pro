import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { Category } from '../Models/Category';
import { PagedResult } from '../Models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

baseURL = environment.apiURL + 'category';

constructor(private http: HttpClient) { }

  public getAllCategories(page: number = 1, pageSize: number = 10,searchTerm: string = ''): Observable<PagedResult<Category>> {
      let params = new HttpParams()
        .set('page', page.toString())
        .set('pageSize', pageSize.toString());

      if (searchTerm && searchTerm.trim() !== '') {
      params = params.set('searchTerm', searchTerm.trim());
    }

  return this.http.get<PagedResult<Category>>(`${this.baseURL}`, { params }).pipe(take(1));
  }


    public getCategoryById(id: number): Observable<Category> {
      return this.http
        .get<Category>(`${this.baseURL}/${id}`)
        .pipe(take(1));
    }

  public insertCategory(evento: Category): Observable<Category> {
    return this.http
      .post<Category>(this.baseURL, evento)
      .pipe(take(1));
  }

   public updateCategory(evento: Category): Observable<Category> {
      return this.http
        .put<Category>(`${this.baseURL}/${evento.id}`, evento)
        .pipe(take(1));
    }

  public deleteCategory(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

}
