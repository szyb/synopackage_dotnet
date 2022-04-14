import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from './config';

@Injectable({
  providedIn: 'root',
})
export class VersionsService {
  constructor(private http: HttpClient) {

  }

  public getDefaultVersion(): Observable<string> {
    return this.http.get<string>(`${Config.apiUrl}Versions/GetDefaultVersion`, { responseType: 'text' as 'json' });
  }
}
