import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { UserSettingsService } from './user-settings.service';
import { HttpClient } from '@angular/common/http';
import { SourceDTO, PackageDTO } from '../sources/sources.model';
import { Config } from './config';
import { Utils } from './Utils';

@Injectable({
    providedIn: 'root',
})
export class ModelsService {
    constructor(private http: HttpClient) {

    }

    public getDefaultModel(): Observable<string> {
        return this.http.get<string>(`${Config.apiUrl}Models/GetDefaultModel`);
    }
}
