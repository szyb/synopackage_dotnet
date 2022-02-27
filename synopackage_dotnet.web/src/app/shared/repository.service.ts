import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SourceDTO, SourcesDTO, SourceServerResponseDTO } from '../sources/sources.model';
import { Config } from './config';
import { Utils } from './Utils';

import { map } from 'rxjs/operators';
import { RepositoryDTO } from '../repository/repository.model';

@Injectable({
  providedIn: 'root',
})
export class RepositoryService {
  constructor(private http: HttpClient) {
  }

  public getRepositoryInfo(version: string): Observable<RepositoryDTO> {
    return this.http.get<RepositoryDTO>(`${Config.apiUrl}Repository/Info/${version}`);
  }

}
