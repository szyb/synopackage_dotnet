import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from './config';
import { NewsPagingDTO, PagingParamsDTO } from './model';
import { Utils } from './Utils';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  private itemsPerPage: number = 5;
  constructor(private http: HttpClient) {
  }

  public getNews(page: number): Observable<NewsPagingDTO> {
    if (page == null)
      return this.http.get<NewsPagingDTO>(`${Config.apiUrl}News/GetNews`);
    else {
      const dto = new PagingParamsDTO();
      dto.page = page;
      dto.itemsPerPage = this.itemsPerPage;
      return this.http.get<NewsPagingDTO>(`${Config.apiUrl}News/GetNews${Utils.getQueryParams(dto)}`)
    }
  }
}
