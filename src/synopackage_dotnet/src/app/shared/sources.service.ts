import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { UserSettingsService } from './user-settings.service';
import { HttpClient } from '@angular/common/http';
import { SourceDTO, PackageDTO, SourcesDTO, SourceServerResponseDTO, SourceLiteDTO } from '../sources/sources.model';
import { Config } from './config';
import { Utils } from './Utils';

@Injectable({
  providedIn: 'root',
})
export class SourcesService {
  constructor(private http: HttpClient, private userSettingsService: UserSettingsService) {

  }

  public getAllSources(): Observable<SourcesDTO> {
    return this.http.get<SourcesDTO>(`${Config.apiUrl}Sources/GetAllSources`);
  }

  public getPackagesFromSource(sourceName: string, model: string, version: string, isBeta: boolean, keyword: string):
    Observable<SourceServerResponseDTO> {
    const params = new SourceBrowseDTO();
    params.sourceName = sourceName;
    params.model = model;
    params.version = version;
    params.isBeta = isBeta;
    params.keyword = keyword;
    return this.http.get<SourceServerResponseDTO>(`${Config.apiUrl}Packages/GetSourceServerResponse${Utils.getQueryParams(params)}`);
  }

  public getAllActiveSources(): Observable<SourceLiteDTO[]> {
    return this.http.get<SourceLiteDTO[]>(`${Config.apiUrl}Sources/GetAllActiveSources`);
  }
}

export class SourceBrowseDTO {
  sourceName: string;
  model: string;
  version: string;
  isBeta: boolean;
  keyword: string;
}
