import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SourceDTO, SourcesDTO } from './sources.model';
import { SourcesService } from '../shared/sources.service';
import { Title } from '@angular/platform-browser';


@Component({
  selector: 'app-sources',
  templateUrl: './sources.component.html',
})
@Injectable()
export class SourcesComponent {
  public sources: SourcesDTO;
  public activeSources: SourceDTO[];
  public inActiveSources: SourceDTO[];

  constructor(http: HttpClient, private sourcesService: SourcesService, private titleService: Title) {
    this.titleService.setTitle('Sources - synopackage.com');
    this.sourcesService.getAllSources().subscribe(result => {
      this.activeSources = result.activeSources;
      this.inActiveSources = result.inActiveSources;
      this.sources = result;
    });
  }
}
