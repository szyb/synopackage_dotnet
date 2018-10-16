import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from '../shared/config';
import { SourceDTO, SourcesDTO } from './sources.model';
import { SourcesService } from '../shared/sources.service';


@Component({
  selector: 'app-sources',
  templateUrl: './sources.component.html',
})
@Injectable()
export class SourcesComponent {
  public sources: SourcesDTO;
  public activeSources: SourceDTO[];
  public inActiveSources: SourceDTO[];

  constructor(http: HttpClient, private sourcesService: SourcesService) {
    this.sourcesService.getAllSources().subscribe(result => {
      this.activeSources = result.activeSources;
      this.inActiveSources = result.inActiveSources;
      this.sources = result;
    });
  }
  // http.get<SourceDTO[]>(`${Config.apiUrl}Sources/GetList`).subscribe( result => {
  //   this.sources = result;
  // });
  // http.get<SourceDTO[]>(Config.apiUrl + 'api/BrowseSource/GetList').subscribe(result => {
  //   this.sources = result;
  // }, error => console.error(error));
}
