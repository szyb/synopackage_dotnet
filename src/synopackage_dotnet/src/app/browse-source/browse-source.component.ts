import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap, Params } from '@angular/router';
import { switchMap, take } from 'rxjs/operators';
import { Config } from '../shared/config';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-browse-source',
  templateUrl: './browse-source.component.html',
})
export class BrowseSourceComponent implements OnInit, OnDestroy {
  private name: Observable<string>;
  private nameString: string;
  constructor(private route: ActivatedRoute,
    private router: Router) {
  }

  private subscription: Subscription;

  ngOnInit() {
    // this.subscription = this.route.params.subscribe((params: Params) => { this.nameString = params['name']; });

    this.route.params.pipe(
      take(1)
    ).subscribe((params: Params) => { this.nameString = params['name']; });

  }
  ngOnDestroy(): void {
    if (this.subscription === null) {
      this.subscription.unsubscribe();
    }
  }
}
