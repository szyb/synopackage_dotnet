import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { UserSettingsService } from './user-settings.service';
import { HttpClient } from '@angular/common/http';
import { SourceDTO, PackageDTO, SourcesDTO } from '../sources/sources.model';
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

  public getPackagesFromSource(sourceName: string, model: string, version: string, isBeta: boolean): Observable<PackageDTO[]> {
    const params = new SourceBrowseDTO();
    params.sourceName = sourceName;
    params.model = model;
    params.version = version;
    params.isBeta = isBeta;
    return this.http.get<PackageDTO[]>(`${Config.apiUrl}Packages/GetList${Utils.getQueryParams(params)}`);
  }
}

export class SourceBrowseDTO {
  sourceName: string;
  model: string;
  version: string;
  isBeta: boolean;
}
