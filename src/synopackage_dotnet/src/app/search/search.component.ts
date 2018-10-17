import { Component, Inject, OnInit, OnDestroy, Injectable } from '@angular/core';
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

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
})
@Injectable()
export class SearchComponent implements OnInit, OnDestroy {
  constructor(private route: ActivatedRoute,
    private sourcesService: SourcesService,
    private userSettingsService: UserSettingsService,
    private modelsService: ModelsService,
    private versionsService: VersionsService,
    private router: Router) {
  }

  public isSearchPerformed: boolean;
  public sources: SourceLiteDTO[];
  public searchResult: SearchResultDTO[];
  private subscription: Subscription;
  public keyword: string;

  ngOnInit() {
    this.searchResult = [];
    this.sourcesService.getAllActiveSources().subscribe(result => {
      this.sources = result;
      this.sources.forEach(item => {
        const sr = new SearchResultDTO();
        sr.name = item.name;
        sr.isSearchEnded = false;
        this.searchResult.push(sr);
      });
    });

    // this.subscription = this.route.params.subscribe((params: Params) => { this.nameString = params['name']; });
    // this.areSettingsSet = this.userSettingsService.isSetup();
    // this.isResponseArrived = false;
    // this.isError = false;
    // this.noPackages = false;
    // this.route.params.pipe(
    //   take(1)
    // ).subscribe((params: Params) => { this.nameString = params['name']; });

    // this.sourcesService.getPackagesFromSource(this.nameString,
    //   this.userSettingsService.getUserModel(),
    //   this.userSettingsService.getUserVersion(),
    //   this.userSettingsService.getUserIsBeta()
    // ).pipe(
    //   take(1)
    // ).subscribe(val => {
    //   this.response = val;
    //   this.result = this.response.result;
    //   this.errorMessage = this.response.errorMessage;
    //   this.isError = !this.result;
    //   this.packages = this.response.packages;
    //   if (this.result && (this.packages == null || this.packages.length === 0)) {
    //     this.noPackages = true;
    //   }
    //   if (this.response.packages != null) {
    //     this.packages.forEach(element => {
    //       element.thumbnailUrl = 'cache/' + element.iconFileName;
    //     });
    //   }
    //   this.isResponseArrived = true;
    // });

  }


  performSearch() {
    if (this.isSearchPerformed) {
      this.isSearchPerformed = false;
      this.searchResult.forEach(item => {
        item.isSearchEnded = false;
        item.noPackages = false;
        item.packages = null;
        item.errorMessage = null;
        item.isValid = false;
        item.response = null;
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
              item.packages.forEach(element => {
                element.thumbnailUrl = 'cache/' + element.iconFileName;
              });
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
