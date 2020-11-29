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
      this.pageParam = parseInt(params['page']);
      if (params['page'] != null && this.pageParam.toString() != params['page']) {
        this.pageParam = 1;
        this.router.navigate(['/info/changelog/page/1']);
      } else {
        if (params['page'] != null && (isNaN(this.pageParam) || this.pageParam <= 0)) {
          this.pageParam = 1;
          this.router.navigate(['/info/changelog/page/1']);
        }
        else if (params['page'] == null) {
          this.pageParam = 1;
        }
        if (this.pageParam != null) {
          this.isPageSetFromParam = true;
        }
      }

      if (this.pageParam != null) {
        this.isPageSetFromParam = true;
      }
    });
    if (this.isPageSetFromParam)
      this.performGetChangelogs(this.pageParam, true);
    else
      this.performGetChangelogs(1, true);
  }

  private performGetChangelogs(page: number, isFirst: boolean) {
    this.changelogService.getChangelogs(page)
      .subscribe((response) => {
        this.changelogsPaging = response;
        this.changelogs = response.items;
        this.dataChanged.next(response);
      },
        (error) => {
          if (error.status === 400) {
            if (isFirst) {
              this.router.navigate(['/info/changelog/page/1']);
              if (this.shouldPerformGetChangelogs())
                this.performGetChangelogs(1, false);
            }
          }
        });
  }

  onPageChange(page: number) {
    let shouldPerformGetChangelogs = false;
    if (page !== this.changelogsPaging.currentPage) {
      shouldPerformGetChangelogs = this.shouldPerformGetChangelogs();
      this.router.navigate(['/info/changelog/page/', page.toString()]);
    }
    if (shouldPerformGetChangelogs) {
      this.changelogsPaging = null;
      this.changelogs = null;
      this.performGetChangelogs(page, true)
    }
  }

  private shouldPerformGetChangelogs(): boolean {
    let shouldPerformGetChangelogs = false;
    if (this.currentRoute !== null) {
      shouldPerformGetChangelogs = this.currentRoute.startsWith('/info/changelog/page');
    }
    return shouldPerformGetChangelogs;
  }

}
