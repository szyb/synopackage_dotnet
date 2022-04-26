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
  public inactiveSources: SourceDTO[];
  public isHealthChecksEnabled: boolean;

  constructor(http: HttpClient, private sourcesService: SourcesService, private titleService: Title) {
    this.titleService.setTitle('Sources - synopackage.com');
    this.sourcesService.healthChecksEnabled().subscribe(result => {
      this.isHealthChecksEnabled = result;
    });
    this.sourcesService.getAllSources().subscribe(result => {
      this.activeSources = result.activeSources;
      this.inactiveSources = result.inactiveSources;
      this.sources = result;
    });

  }
}
