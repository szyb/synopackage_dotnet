import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from './config';
import { ChangelogDTO } from './model';

@Injectable({
  providedIn: 'root',
})
export class ChangelogService {
  constructor(private http: HttpClient) {
  }

  public getChangelogs(): Observable<ChangelogDTO[]> {
    return this.http.get<ChangelogDTO[]>(`${Config.apiUrl}Changelog/GetChangelogs`);
  }
}
