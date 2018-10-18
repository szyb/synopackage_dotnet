import { Component, Inject, OnInit, OnDestroy, Injectable, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap, Params } from '@angular/router';
import { switchMap, take } from 'rxjs/operators';
import { Config } from '../shared/config';
import { Observable, Subscription } from 'rxjs';
import { PackageDTO, SourceServerResponseDTO } from '../sources/sources.model';
import { SourcesService } from '../shared/sources.service';
import { UserSettingsService } from '../shared/user-settings.service';
import { ModelsService } from '../shared/models.service';
import { VersionsService } from '../shared/versions.service';
import { PackageInfoComponent } from '../components/package-info/package-info.component';

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
  constructor(private route: ActivatedRoute,
    private sourcesService: SourcesService,
    private userSettingsService: UserSettingsService,
    private modelsService: ModelsService,
    private versionsService: VersionsService,
    private router: Router) {
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
    this.route.params.pipe(
      take(1)
    ).subscribe((params: Params) => { this.nameString = params['name']; });

    this.sourcesService.getPackagesFromSource(this.nameString,
      this.userSettingsService.getUserModel(),
      this.userSettingsService.getUserVersion(),
      this.userSettingsService.getUserIsBeta(),
      null
    ).pipe(
      take(1)
    ).subscribe(val => {
      this.response = val;
      this.result = this.response.result;
      this.errorMessage = this.response.errorMessage;
      this.isError = !this.result;
      this.packages = this.response.packages;
      if (this.result && (this.packages == null || this.packages.length === 0)) {
        this.noPackages = true;
      }
      if (this.response.packages != null) {
        this.packages.forEach(element => {
          element.thumbnailUrl = 'cache/' + element.iconFileName;
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
