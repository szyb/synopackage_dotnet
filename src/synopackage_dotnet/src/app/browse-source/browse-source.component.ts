import { Component, Inject, OnInit, OnDestroy, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap, Params } from '@angular/router';
import { switchMap, take } from 'rxjs/operators';
import { Config } from '../shared/config';
import { Observable, Subscription } from 'rxjs';
import { PackageDTO } from '../sources/sources.model';
import { SourcesService } from '../shared/sources.service';
import { UserSettingsService } from '../shared/user-settings.service';

@Component({
  selector: 'app-browse-source',
  templateUrl: './browse-source.component.html',
})
@Injectable()
export class BrowseSourceComponent implements OnInit, OnDestroy {
  private name: Observable<string>;
  public nameString: string;
  public packages: PackageDTO[];
  constructor(private route: ActivatedRoute,
    private sourcesService: SourcesService,
    private userSettingsService: UserSettingsService,
    private router: Router) {
  }

  private subscription: Subscription;

  ngOnInit() {
    // this.subscription = this.route.params.subscribe((params: Params) => { this.nameString = params['name']; });

    this.route.params.pipe(
      take(1)
    ).subscribe((params: Params) => { this.nameString = params['name']; });

    this.sourcesService.getPackagesFromSource(this.nameString,
      this.userSettingsService.getUserModel(),
      this.userSettingsService.getUserVersion(),
      this.userSettingsService.getUserIsBeta()
    ).pipe(
      take(1)
    ).subscribe(val => {
      this.packages = val;
      this.packages.forEach(element => {
        element.thumbnailUrl = 'cache/' + element.iconFileName;
      });
    });

  }
  ngOnDestroy(): void {
    if (this.subscription === null) {
      this.subscription.unsubscribe();
    }
  }
}
