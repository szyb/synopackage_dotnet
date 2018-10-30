import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { UserSettingsService } from './user-settings.service';
import { HttpClient } from '@angular/common/http';
import { SourceDTO, PackageDTO, SourcesDTO, SourceServerResponseDTO, SourceLiteDTO } from '../sources/sources.model';
import { Config } from './config';
import { Utils } from './Utils';
import { NewsDTO } from './model';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  constructor(private http: HttpClient, private userSettingsService: UserSettingsService) {
  }

  public getNews(): Observable<NewsDTO> {
    return this.http.get<NewsDTO>(`${Config.apiUrl}News/GetNews`);
  }
}
