import { Component, OnInit, ViewChild } from '@angular/core';
import { NewsDTO, NewsPagingDTO, PagingDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';
import { NewsService } from '../shared/news.service';
import { PaginatorComponent } from '../components/paginator/paginator.component';
import { PaginatorDataProvider } from '../components/paginator/paginator-data';
import { Subject } from 'rxjs';
import { ActivatedRoute, NavigationEnd, Params, Router } from '@angular/router';
import { take } from 'rxjs/operators';


@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
})

export class NewsComponent implements OnInit, PaginatorDataProvider {
  public newsPaging: NewsPagingDTO;
  public news: NewsDTO[];
  private currentRoute: string;
  private pageParam: number;
  private isPageSetFromParam: boolean = false;

  constructor(private titleService: Title, private newsService: NewsService, private router: Router, private route: ActivatedRoute) {
    this.titleService.setTitle('News - synopackage.com');
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });
  }

  public dataChanged: Subject<PagingDTO> = new Subject<PagingDTO>();

  @ViewChild(PaginatorComponent, { static: true })
  paginator: PaginatorComponent;

  ngOnInit(): void {
    this.route.params.pipe(
      take(1)
    ).subscribe((params: Params) => {
      this.pageParam = parseInt(params['page']);
      if (params['page'] != null && this.pageParam.toString() != params['page']) {
        this.pageParam = 1;
        this.router.navigate(['/news/page/1']);
      } else {
        if (params['page'] != null && (isNaN(this.pageParam) || this.pageParam <= 0)) {
          this.pageParam = 1;
          this.router.navigate(['/news/page/1']);
        }
        else if (params['page'] == null) {
          this.pageParam = 1;
        }
        if (this.pageParam != null) {
          this.isPageSetFromParam = true;
        }
      }
    });
    if (this.isPageSetFromParam)
      this.performGetNews(this.pageParam, true);
    else
      this.performGetNews(1, true);

  }

  private performGetNews(page: number, isFirst: boolean) {
    let isError = false;
    this.newsService.getNews(page)
      .subscribe((response) => {
        this.newsPaging = response;
        this.news = response.items;
        this.dataChanged.next(response);
      },
        (error) => {
          if (error.status === 400) {
            if (isFirst) {
              this.router.navigate(['/news/page/1']);
              if (this.shouldPerformGetNews())
                this.performGetNews(1, false);
            }
          }
        });

  }

  onPageChange(page: string) {
    let shouldPerformGetNews = false;
    if (page !== this.newsPaging.currentPage) {
      shouldPerformGetNews = this.shouldPerformGetNews();
      this.router.navigate(['/news/page/', page.toString()]);
    }
    if (shouldPerformGetNews) {
      this.newsPaging = null;
      this.news = null;
      this.performGetNews(eval(page), true)
    }
  }

  private shouldPerformGetNews(): boolean {
    let shouldPerformGetNews = false;
    if (this.currentRoute !== null) {
      shouldPerformGetNews = this.currentRoute.startsWith('/news/page');
    }
    return shouldPerformGetNews;
  }

}
