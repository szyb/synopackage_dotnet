import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Title } from '@angular/platform-browser';
import { RepositoryService } from '../shared/repository.service';
import { RepositoryDTO } from './repository.model';
import { Config } from '../shared/config';
import { UserSettingsService } from '../shared/user-settings.service';


@Component({
  selector: 'app-repository',
  templateUrl: './repository.component.html',
  styleUrls: ['repository.component.scss']
})
@Injectable()
export class RepositoryComponent {
  public repoDetails: RepositoryDTO;
  public baseUrl: string;

  constructor(http: HttpClient,
    private repositoryService: RepositoryService,
    private titleService: Title,
    private userSettingsService: UserSettingsService,

  ) {
    this.baseUrl = Config.baseUrl.substr(0, Config.baseUrl.length - 1);
    this.titleService.setTitle('Repository - synopackage.com');
    const version = userSettingsService.getUserVersion();
    this.repositoryService.getRepositoryInfo(version).subscribe(result => {
      this.repoDetails = result;
    });
  }
}
