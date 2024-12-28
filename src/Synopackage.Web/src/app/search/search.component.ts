import { Component, OnInit, OnDestroy, Injectable, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, Params, NavigationEnd, NavigationStart } from '@angular/router';
import { take } from 'rxjs/operators';
import { Config } from '../shared/config';
import { Observable, Subscription } from 'rxjs';
import { SourceDTO } from '../sources/sources.model';
import { SourcesService } from '../shared/sources.service';
import { UserSettingsService } from '../shared/user-settings.service';
import { SearchResultDTO } from './search.model';
import { PackageInfoComponent } from '../components/package-info/package-info.component';
import { ParametersDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';
import { Utils } from '../shared/Utils';

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
    private titleService: Title,
    private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });
    this.titleService.setTitle('Search - synopackage.com');
  }

  public isSearchLinkCollapsed = true;
  public linksAvailable = false;
  public shortLink: string;
  public fullLink: string;
  public isSearchPerformed: boolean;
  public areSettingsSet: boolean;
  public sources: SourceDTO[];
  public searchResult: SearchResultDTO[];

  public keyword: string;
  public parameters: ParametersDTO;
  private keywordParam: string;
  private modelParam: string;
  private versionParam: string;
  private channelParam: string;
  private currentRoute: string;

  public expandIcon = 'fa fa-eye';
  public collapseIcon = 'fa fa-eye-slash';

  @ViewChild(PackageInfoComponent, { static: true })
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
      if (this.keywordParam != null && this.keywordParam !== '*') {
        this.keyword = this.keywordParam.substring(0, 300);
      }
    });
    this.searchResult = [];
    this.areSettingsSet = this.userSettingsService.isSetup();
    this.sourcesService.getAllActiveSources().subscribe(result => {
      this.sources = result;
      this.sources.forEach(item => {
        const sr = new SearchResultDTO();
        sr.name = item.name;
        sr.url = item.url;
        sr.www = item.www;
        sr.isSearchEnded = false;
        this.searchResult.push(sr);
      });
      if (areParamsSet) {
        this.performSearch();
      }
    });
  }

  onSearchButton() {
    this.performAnotherSearch();
  }

  onEnter() {
    this.performAnotherSearch();
  }

  performAnotherSearch() {
    if (this.keyword == null || this.keyword === '') {
      this.keyword = '*';
    }
    this.keywordParam = null;
    this.modelParam = null;
    this.versionParam = null;
    this.channelParam = null;
    this.parameters = null;
    let shouldPerformSearch: boolean = false;
    //if router link does not starts with '/search/keyword' then we don't need to performSearch()
    //beacuse it will be handled on next ngInit()
    if (this.keyword != null && this.keyword !== '') {
      shouldPerformSearch = this.shouldPerformSearch();
      this.router.navigate(['/search/keyword', this.keyword]);
    } else {
      this.router.navigate(['/search']);
    }
    if (shouldPerformSearch) {
      this.performSearch();
    }
    if (this.keyword === '*')
      this.keyword = null;
  }


  private shouldPerformSearch(): boolean {
    let shouldPerformSearch = false;
    if (this.currentRoute !== null) {
      const lastSlashIndex = this.currentRoute.lastIndexOf('/');
      if (lastSlashIndex === 15) {
        //if '/' is only one in current route or there are more than 3 slashes
        shouldPerformSearch = true;
      }
    }
    return shouldPerformSearch;
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
        item.isCollapsed = true;
        item.sourceInfo = null;
        item.isDownloadDisabled = false;
      });
    }

    const { keywordForSearch, model, version, channel } = this.getParameters();

    if (keywordForSearch != null && keywordForSearch !== '') {
      this.titleService.setTitle('Search for "' + keywordForSearch + '" - synopackage.com');
    }
    if (this.searchResult != null) {
      this.searchResult.forEach(item => {
        this.sourcesService.getPackagesFromSource(item.name,
          model,
          version,
          channel,
          keywordForSearch,
          true
        )
          .pipe(
            take(1)
          ).subscribe(val => {
            item.packages = val.packages;
            item.isValid = val.result;
            item.errorMessage = val.errorMessage;
            item.isSearchEnded = true;
            item.isCollapsed = false;
            if (item.isValid && (item.packages == null || item.packages.length === 0)) {
              item.noPackages = true;
            }
            if (item.packages != null) {
              item.count = item.packages.length;
              item.packages.forEach(element => {
                element.thumbnailUrl = Config.cacheFolder + element.iconFileName;
              });
            }
            if (val.resultFrom === 3 && val.cacheOld !== null) {
              item.isExpiredCache = true;
              item.cacheOldString = Utils.getCacheOldString(val.cacheOld);
            }
            if (this.parameters == null) {
              this.parameters = val.parameters;
              this.generateSearchLinks(keywordForSearch, model, version, channel);
            }
            item.sourceInfo = val.sourceInfo;
            item.isDownloadDisabled = val.isDownloadDisabled;
          });
      });
      this.isSearchPerformed = true;
    }
  }

  private getParameters() {
    let model = this.modelParam != null ? this.modelParam : this.userSettingsService.getUserModel();
    let version = this.versionParam != null ? this.versionParam : this.userSettingsService.getUserVersion();
    const channel = this.channelParam === 'beta' ? true : this.userSettingsService.getUserIsBeta();
    let keywordForSearch = this.keywordParam != null && this.keywordParam !== '*' ? this.keywordParam : this.keyword;
    if (keywordForSearch === '*')
      keywordForSearch = null;

    if (model != null) {
      model = model.substring(0, 100);
    }
    if (version != null) {
      version = version.substring(0, 100);
    }
    if (keywordForSearch != null) {
      keywordForSearch = keywordForSearch.substring(0, 300);
    }
    return { keywordForSearch, model, version, channel };
  }

  generateSearchLinks(keyword: string, model: string, version: string, channel: boolean) {
    if (model === null && this.parameters != null) {
      model = this.parameters.model != null ? this.parameters.model : this.parameters.modelOrUnique;
    }
    if (version === null && this.parameters != null) {
      version = this.parameters.version;
    }
    if (keyword != null && keyword !== '') {
      this.linksAvailable = true;
      this.shortLink = `${Config.baseUrl}search/keyword/` + encodeURIComponent(keyword);
      const channelString = channel === false ? 'stable' : 'beta';
      this.fullLink = `${Config.baseUrl}search/keyword/` + encodeURIComponent(keyword) + '/model/'
        + encodeURIComponent(model) + '/version/' + encodeURIComponent(version) + '/channel/' + channelString;
    } else if (keyword === '' || keyword === '*' || keyword === undefined || keyword === null) {
      this.linksAvailable = true;
      this.shortLink = `${Config.baseUrl}search/keyword/*`;
      const channelString = channel === false ? 'stable' : 'beta';
      this.fullLink = `${Config.baseUrl}search/keyword/*/model/`
        + encodeURIComponent(model) + '/version/' + encodeURIComponent(version) + '/channel/' + channelString;

    } else {
      this.linksAvailable = false;
      this.shortLink = 'unavailable';
      this.fullLink = 'unavailable';
    }
  }

  ngOnDestroy(): void {

  }

  getIcon(isCollapsed) {
    if (isCollapsed) {
      return this.collapseIcon;
    } else {
      return this.expandIcon;
    }
  }

  changeState(result: any) {
    result.isCollapsed = !result.isCollapsed;
  }
}
