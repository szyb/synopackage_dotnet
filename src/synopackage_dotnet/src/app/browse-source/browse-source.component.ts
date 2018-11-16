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

@Component({
  selector: 'app-browse-source',
  templateUrl: './browse-source.component.html',
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
  public count: number;
  constructor(private route: ActivatedRoute,
    private sourcesService: SourcesService,
    private userSettingsService: UserSettingsService,
    private titleService: Title,
    private router: Router) {
    this.titleService.setTitle('Browse source - synopackage.com');
  }

  private subscription: Subscription;

  @ViewChild(PackageInfoComponent)
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
    });



    this.sourcesService.getPackagesFromSource(this.nameString,
      this.userSettingsService.getUserModel(),
      this.userSettingsService.getUserVersion(),
      this.userSettingsService.getUserIsBeta(),
      null,
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
      this.isResponseArrived = true;
    });

  }
  ngOnDestroy(): void {
    if (this.subscription === null) {
      this.subscription.unsubscribe();
    }
  }
}
