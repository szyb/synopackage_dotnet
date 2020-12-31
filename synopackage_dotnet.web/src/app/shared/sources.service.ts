import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SourceDTO, SourcesDTO, SourceServerResponseDTO } from '../sources/sources.model';
import { Config } from './config';
import { Utils } from './Utils';
import { ParametersDTO, DownloadRequestDTO } from './model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class SourcesService {
  constructor(private http: HttpClient) {

  }

  public getAllSources(): Observable<SourcesDTO> {
    return this.http.get<SourcesDTO>(`${Config.apiUrl}Sources/GetAllSources`);
  }

  public getPackagesFromSource(sourceName: string, model: string, version: string, isBeta: boolean, keyword: string, isSearch: boolean):
    Observable<SourceServerResponseDTO> {
    const params = new ParametersDTO();
    params.sourceName = sourceName;
    params.model = model;
    params.version = version;
    params.isBeta = isBeta;
    params.keyword = keyword;
    params.isSearch = isSearch;
    return this.http.get<SourceServerResponseDTO>(`${Config.apiUrl}Packages/GetSourceServerResponse${Utils.getQueryParams(params)}`);
  }

  public getAllActiveSources(): Observable<SourceDTO[]> {
    return this.http.get<SourceDTO[]>(`${Config.apiUrl}Sources/GetAllActiveSources`);
  }

  public getSource(sourceName: string): Observable<SourceDTO> {
    const params: any = {
      sourceName: sourceName
    };
    return this.http.get<SourceDTO>(`${Config.apiUrl}Sources/GetSource${Utils.getQueryParams(params)}`);
  }

}
