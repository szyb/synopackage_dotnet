import { Component, OnInit, OnDestroy, Injectable, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { take } from 'rxjs/operators';
import { Config } from '../shared/config';
import { Observable, Subscription } from 'rxjs';
import { PackageDTO, SourceServerResponseDTO } from '../sources/sources.model';
import { SourcesService } from '../shared/sources.service';
import { UserSettingsService } from '../shared/user-settings.service';
import { PackageInfoComponent } from '../components/package-info/package-info.component';
import { ParametersDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';
import { Utils } from '../shared/Utils';

@Component({
  selector: 'app-browse-source',
  templateUrl: './browse-source.component.html',
  styleUrls: ['./browse-source.component.scss']
})
@Injectable()
export class BrowseSourceComponent implements OnInit, OnDestroy {
  private name: Observable<string>;
  public nameString: string;
  public packages: PackageDTO[];
  public response: SourceServerResponseDTO;
  public isResponseArrived: boolean;
  public isError: boolean;
  public result: boolean;
  public errorMessage: string;
  public areSettingsSet: boolean;
  public noPackages: boolean;
  public parameters: ParametersDTO;
  public sourceUrl: string;
  public isOfficial: boolean;
  public sourceWww: string;
  public count: number;
  public isSearchLinkCollapsed = true;
  public linksAvailable = false;
  public shortLink: string;
  public fullLink: string;
  public isExpiredCache: boolean;
  public cacheOldString: string;
  public info: string;
  public isDownloadDisabled: boolean;
  private keywordParam: string;
  private modelParam: string;
  private versionParam: string;
  private channelParam: string;
  constructor(private route: ActivatedRoute,
    private sourcesService: SourcesService,
    private userSettingsService: UserSettingsService,
    private titleService: Title,
    private router: Router) {
    this.titleService.setTitle('Browse source - synopackage.com');
  }

  private subscription: Subscription;

  @ViewChild(PackageInfoComponent, { static: true })
  PackageInfoComponent: PackageInfoComponent;

  ngOnInit() {
    // this.subscription = this.route.params.subscribe((params: Params) => { this.nameString = params['name']; });


    this.areSettingsSet = this.userSettingsService.isSetup();
    this.isResponseArrived = false;
    this.isError = false;
    this.noPackages = false;
    this.parameters = new ParametersDTO();


    this.count = 0;
    this.route.params.pipe(
      take(1)
    ).subscribe((params: Params) => {
      this.nameString = params['name'];
      this.titleService.setTitle('Browse source - ' + this.nameString + ' - synopackage.com');
      this.keywordParam = params['keyword'];
      this.modelParam = params['model'];
      this.versionParam = params['version'];
      this.channelParam = params['channel'];
    });

    this.sourcesService.getSource(this.nameString)
      .pipe(
        take(1)
      ).subscribe(val => {
        this.sourceUrl = val.displayUrl;
        this.sourceWww = val.www;
        this.isOfficial = val.isOfficial;
        this.info = val.info;
        this.isDownloadDisabled = val.isDownloadDisabled;
      });
    const { keywordForSearch, model, version, channel } = this.getParameters();

    this.sourcesService.getPackagesFromSource(this.nameString,
      model,
      version,
      channel,
      keywordForSearch,
      false
    ).pipe(
      take(1)
    ).subscribe(val => {
      this.response = val;
      this.result = this.response.result;
      this.errorMessage = this.response.errorMessage;
      this.isError = !this.result;
      this.packages = this.response.packages;
      this.parameters = this.response.parameters;
      if (this.result && (this.packages == null || this.packages.length === 0)) {
        this.noPackages = true;
      }
      if (this.response.packages != null) {
        this.count = this.response.packages.length;
        this.packages.forEach(element => {
          element.thumbnailUrl = Config.cacheFolder + element.iconFileName;
        });
      }
      if (this.response.resultFrom === 3 && this.response.cacheOld !== null) {
        this.isExpiredCache = true;
        this.cacheOldString = Utils.getCacheOldString(this.response.cacheOld);
      }
      this.isResponseArrived = true;
      this.generateSearchLinks(this.nameString, keywordForSearch, model, version, channel);
    });
  }

  private getParameters() {
    let model = this.modelParam != null ? this.modelParam : this.userSettingsService.getUserModel();
    let version = this.versionParam != null ? this.versionParam : this.userSettingsService.getUserVersion();
    const channel = this.channelParam === 'beta' ? true : this.userSettingsService.getUserIsBeta();
    let keywordForSearch = this.keywordParam != null && this.keywordParam !== '*' ? this.keywordParam : null;
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

  generateSearchLinks(sourceName: string, keyword: string, model: string, version: string, channel: boolean) {
    if (model === null && this.parameters != null) {
      model = this.parameters.model != null ? this.parameters.model : this.parameters.modelOrUnique;
    }
    if (version === null && this.parameters != null) {
      version = this.parameters.version;
    }
    if (keyword != null && keyword !== '') {
      this.linksAvailable = true;
      this.shortLink = `${Config.baseUrl}sources/` + encodeURIComponent(sourceName) + `/keyword/` + encodeURIComponent(keyword);
      const channelString = channel === false ? 'stable' : 'beta';
      this.fullLink = `${Config.baseUrl}sources/` + encodeURIComponent(sourceName) + `/keyword/` + encodeURIComponent(keyword) + '/model/'
        + encodeURIComponent(model) + '/version/' + encodeURIComponent(version) + '/channel/' + channelString;
    } else if (keyword === '' || keyword === '*' || keyword === undefined || keyword === null) {
      this.linksAvailable = true;
      this.shortLink = `${Config.baseUrl}sources/` + encodeURIComponent(sourceName) + `/keyword/*`;
      const channelString = channel === false ? 'stable' : 'beta';
      this.fullLink = `${Config.baseUrl}sources/` + encodeURIComponent(sourceName) + `/keyword/*/model/`
        + encodeURIComponent(model) + '/version/' + encodeURIComponent(version) + '/channel/' + channelString;

    } else {
      this.linksAvailable = false;
      this.shortLink = 'unavailable';
      this.fullLink = 'unavailable';
    }
  }
  ngOnDestroy(): void {
    if (this.subscription === null) {
      this.subscription.unsubscribe();
    }
  }

}
