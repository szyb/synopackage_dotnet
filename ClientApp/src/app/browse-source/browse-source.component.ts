import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'browse-source',
  templateUrl: './browse-source.component.html',
})
export class BrowseSourceComponent {
  public sources: SourceDTO[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<SourceDTO[]>(baseUrl + 'api/BrowseSource/GetList').subscribe(result => {
      this.sources = result;
    }, error => console.error(error));
  }
}


interface SourceDTO {
  name: string;
  url: string;
  www: string;
}
