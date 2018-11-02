import { Component, Inject, OnInit, OnDestroy, Injectable, ViewChild, ViewChildren } from '@angular/core';
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
import { CollapseDirective } from 'angular-bootstrap-md';

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

  public isSearchLinkCollapsed = false;
  public shortLink: string;
  public fullLink: string;

  public isSearchPerformed: boolean;
  public areSettingsSet: boolean;
  public sources: SourceLiteDTO[];
  public searchResult: SearchResultDTO[];
  private subscription: Subscription;
  public keyword: string;
  public parameters: ParametersDTO;
  private keywordParam: string;
  private modelParam: string;
  private versionParam: string;
  private channelParam: string;

  @ViewChild(PackageInfoComponent)
  PackageInfoComponent: PackageInfoComponent;

  ngOnInit() {
    let areParamsSet = false;
    this.route.params.pipe(
      take(1)
    ).subscribe((params: Params) => {
      this.keywordParam = params['keyword'];
      this.modelParam = params['model'];
      this.versionParam = params['version'];
      this.channelParam = params['channel'];
      if (this.keywordParam != null ||
        this.modelParam != null ||
        this.versionParam != null ||
        this.channelParam != null) {
        areParamsSet = true;
      }
      if (this.keywordParam != null) {
        this.keyword = this.keywordParam;
      }
    });
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
      if (areParamsSet) {
        this.performSearch();
      }
    });
  }

  clearLinkParams() {
    this.keywordParam = null;
    this.modelParam = null;
    this.versionParam = null;
    this.channelParam = null;
    this.router.navigate(['/search/keyword', this.keyword]);
  }

  onSearchButton() {
    this.clearLinkParams();
    this.performSearch();
  }

  onEnter() {
    this.clearLinkParams();
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
    const model = this.modelParam != null ? this.modelParam : this.userSettingsService.getUserModel();
    const version = this.versionParam != null ? this.versionParam : this.userSettingsService.getUserVersion();
    const channel = this.channelParam === 'beta' ? true : this.userSettingsService.getUserIsBeta();
    const keywordForSearch = this.keywordParam != null ? this.keywordParam : this.keyword;

    this.generateSearchLinks(keywordForSearch, model, version, channel);

    if (keywordForSearch != null && keywordForSearch !== '') {
      this.titleService.setTitle('Search for "' + keywordForSearch + '" - synopackage.com');
    }
    if (this.searchResult != null) {
      this.searchResult.forEach(item => {
        this.sourcesService.getPackagesFromSource(item.name,
          model,
          version,
          channel,
          keywordForSearch
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

  generateSearchLinks(keyword: string, model: string, version: string, channel: boolean) {
    this.shortLink = `${Config.baseUrl}search/keyword/` + keyword;
    const channelString = channel === false ? 'stable' : 'beta';
    this.fullLink = `${Config.baseUrl}search/keyword/` + keyword + '/model/' + model + '/version/' + version + '/channel/' + channelString;
  }

  ngOnDestroy(): void {
    if (this.subscription === null) {
      this.subscription.unsubscribe();
    }
  }
}
