import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from './config';
import { ChangelogPagingDTO, PagingParamsDTO } from './model';
import { Utils } from './Utils';

@Injectable({
  providedIn: 'root',
})
export class ChangelogService {
  private itemsPerPage: number = 5;
  constructor(private http: HttpClient) {
  }

  public getChangelogs(page: number): Observable<ChangelogPagingDTO> {
    if (page == null)
      return this.http.get<ChangelogPagingDTO>(`${Config.apiUrl}Changelog/GetChangelogs`);
    else {
      const dto = new PagingParamsDTO();
      dto.page = page;
      dto.itemsPerPage = this.itemsPerPage;
      return this.http.get<ChangelogPagingDTO>(`${Config.apiUrl}Changelog/GetChangelogs${Utils.getQueryParams(dto)}`)
    }
  }
}
