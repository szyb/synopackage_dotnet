import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { UserSettingsService } from './user-settings.service';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Config } from './config';
import { Utils } from './Utils';

@Injectable({
  providedIn: 'root',
})
export class ModelsService {
  constructor(private http: HttpClient) {

  }

  public getDefaultModel(): Observable<string> {
    return this.http.get<string>(`${Config.apiUrl}Models/GetDefaultModel`, { responseType: 'text' as 'json' });
  }
}
