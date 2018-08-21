import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from '../shared/config';


@Component({
  selector: 'app-sources',
  templateUrl: './sources.component.html',
})
export class SourcesComponent {
  public sources: SourceDTO[];

  constructor(http: HttpClient) {
    http.get<SourceDTO[]>(`${Config.apiUrl}Sources/GetList`).subscribe( result => {
      this.sources = result;
    });
    // http.get<SourceDTO[]>(Config.apiUrl + 'api/BrowseSource/GetList').subscribe(result => {
    //   this.sources = result;
    // }, error => console.error(error));
  }
}


interface SourceDTO {
  name: string;
  url: string;
  www: string;
}
