import { Component, Inject, OnInit, OnDestroy, Injectable, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap, Params } from '@angular/router';
import { switchMap, take } from 'rxjs/operators';
import { Config } from '../shared/config';
import { Observable, Subscription } from 'rxjs';
import { PackageDTO, SourceServerResponseDTO, SourceLiteDTO } from '../sources/sources.model';
import { SourcesService } from '../shared/sources.service';
import { UserSettingsService } from '../shared/user-settings.service';
import { ModelsService } from '../shared/models.service';
import { VersionsService } from '../shared/versions.service';
import { SearchResultDTO } from './search.model';
import { PackageInfoComponent } from '../components/package-info/package-info.component';
import { ParametersDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
@Injectable()
export class SearchComponent implements OnInit, OnDestroy {
  constructor(private route: ActivatedRoute,
    private sourcesService: SourcesService,
    private userSettingsService: UserSettingsService,
    private modelsService: ModelsService,
    private versionsService: VersionsService,
    private titleService: Title,
    private router: Router) {
    this.titleService.setTitle('Search - synopackage.com');
  }

  public isSearchPerformed: boolean;
  public areSettingsSet: boolean;
  public sources: SourceLiteDTO[];
  public searchResult: SearchResultDTO[];
  private subscription: Subscription;
  public keyword: string;
  public parameters: ParametersDTO;

  @ViewChild(PackageInfoComponent)
  PackageInfoComponent: PackageInfoComponent;

  ngOnInit() {
    this.searchResult = [];
    this.areSettingsSet = this.userSettingsService.isSetup();
    this.sourcesService.getAllActiveSources().subscribe(result => {
      this.sources = result;
      this.sources.forEach(item => {
        const sr = new SearchResultDTO();
        sr.name = item.name;
        sr.isSearchEnded = false;
        this.searchResult.push(sr);
      });
    });
  }

  onEnter() {
    this.performSearch();
  }

  performSearch() {
    if (this.isSearchPerformed) {
      this.isSearchPerformed = false;
      this.parameters = null;
      this.searchResult.forEach(item => {
        item.isSearchEnded = false;
        item.noPackages = false;
        item.packages = null;
        item.errorMessage = null;
        item.isValid = false;
        item.response = null;
        item.count = 0;
      });
    }
    if (this.searchResult != null) {
      this.searchResult.forEach(item => {
        this.sourcesService.getPackagesFromSource(item.name,
          this.userSettingsService.getUserModel(),
          this.userSettingsService.getUserVersion(),
          this.userSettingsService.getUserIsBeta(),
          this.keyword
        )
          .pipe(
            take(1)
          ).subscribe(val => {
            item.packages = val.packages;
            item.isValid = val.result;
            item.errorMessage = val.errorMessage;
            item.isSearchEnded = true;
            if (item.isValid && (item.packages == null || item.packages.length === 0)) {
              item.noPackages = true;
            }
            if (item.packages != null) {
              item.count = item.packages.length;
              item.packages.forEach(element => {
                element.thumbnailUrl = 'cache/' + element.iconFileName;
              });
            }
            if (this.parameters == null) {
              this.parameters = val.parameters;
            }
          });
      });
      this.isSearchPerformed = true;
    }
  }

  ngOnDestroy(): void {
    if (this.subscription === null) {
      this.subscription.unsubscribe();
    }
  }
}