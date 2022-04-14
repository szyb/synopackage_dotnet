import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Config } from './config';
import { DownloadRequestDTO } from './model';

@Injectable({
  providedIn: 'root',
})

export class DownloadSpkService {
  constructor(private http: HttpClient) {

  }

  public downloadRequest(requestUrl: string, sourceName: string, packageName: string): Observable<string> {
    const request = new DownloadRequestDTO();
    request.requestUrl = requestUrl;
    request.sourceName = sourceName;
    request.packageName = packageName;
    const body = JSON.stringify(request);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<string>(`${Config.apiUrl}Download/DownloadRequest`, body, { headers: headers, responseType: 'text' as 'json' });

  }
}
