import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from './config';

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
