import { Component, OnInit } from '@angular/core';
import { ChangelogDTO, ChangelogPagingDTO, PagingDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';
import { ChangelogService } from '../shared/changelog.service';
import { PaginatorDataProvider } from '../components/paginator/paginator-data';
import { Subject } from 'rxjs';
import { ActivatedRoute, NavigationEnd, Params, Router } from '@angular/router';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-changelog',
  templateUrl: './changelog.component.html',
})

export class ChangelogComponent implements OnInit, PaginatorDataProvider {

  public changelogs: ChangelogDTO[];
  private currentRoute: string;
  private changelogsPaging: ChangelogPagingDTO;
  private pageParam: number;
  private isPageSetFromParam: boolean;


  constructor(private titleService: Title, private changelogService: ChangelogService, private router: Router, private route: ActivatedRoute) {
    this.titleService.setTitle('Changelog - synopackage.com');
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });
  }

  public dataChanged: Subject<PagingDTO> = new Subject<PagingDTO>();

  ngOnInit(): void {
    this.route.params.pipe(
      take(1)
    ).subscribe((params: Params) => {
      this.pageParam = params['page'];
      if (this.pageParam != null) {
        this.isPageSetFromParam = true;
      }
    });
    if (this.isPageSetFromParam)
      this.performGetChangelogs(this.pageParam);
    else
      this.performGetChangelogs(1);
  }

  private performGetChangelogs(page: number) {
    this.changelogService.getChangelogs(page)
      .subscribe(val => {
        this.changelogsPaging = val;
        this.changelogs = val.items;
        this.dataChanged.next(val);
      });
  }

  onPageChange(page: number) {
    let shouldPerformGetNews = false;
    if (page !== this.changelogsPaging.currentPage) {
      shouldPerformGetNews = this.shouldPerformGetChangelogs();
      this.router.navigate(['/info/changelog/page/', page.toString()]);
    }
    if (shouldPerformGetNews) {
      this.changelogsPaging = null;
      this.changelogs = null;
      this.performGetChangelogs(page)
    }
  }

  private shouldPerformGetChangelogs(): boolean {
    let shouldPerformGetNews = false;
    if (this.currentRoute !== null) {
      shouldPerformGetNews = this.currentRoute.startsWith('/info/changelog/page');
    }
    return shouldPerformGetNews;
  }

}
