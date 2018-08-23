import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { UserSettingsService } from './user-settings.service';
import { HttpClient } from '@angular/common/http';
import { SourceDTO, PackageDTO } from '../sources/sources.model';
import { Config } from './config';

@Injectable({
    providedIn: 'root',
  })
export class SourcesService {
    constructor(private http: HttpClient, private userSettingsService: UserSettingsService) {

    }

    public getAllSources(): Observable<SourceDTO[]> {
        console.log('getAllSources');
        return this.http.get<SourceDTO[]>(`${Config.apiUrl}Sources/GetList`);
    }

    // public getPackagesFromSource(sourceName: string; model: string, version: string): Observable<PackageDTO[]> {
    //     this.http.get<PackageDTO[]>(`${Config.apiUrl}Pacakges/GetList`);
    // }

}
