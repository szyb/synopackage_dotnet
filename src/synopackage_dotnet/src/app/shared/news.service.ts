import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { UserSettingsService } from './user-settings.service';
import { HttpClient } from '@angular/common/http';
import { Config } from './config';
import { NewsDTO } from './model';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  constructor(private http: HttpClient) {
  }

  public getNews(): Observable<NewsDTO> {
    return this.http.get<NewsDTO>(`${Config.apiUrl}News/GetNews`);
  }
}
