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
  public shouldDisplayPaginator: boolean = true;
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
      this.pageParam = params['page'];
      if (this.pageParam != null) {
        this.isPageSetFromParam = true;
      }
    });
    if (this.isPageSetFromParam)
      this.performGetNews(this.pageParam);
    else
      this.performGetNews(1);

  }

  private performGetNews(page: number) {
    this.newsService.getNews(page)
      .subscribe(val => {
        this.newsPaging = val;
        this.news = val.items;
        this.dataChanged.next(val);
      });
  }

  onPageChange(page: number) {
    let shouldPerformGetNews = false;
    if (page !== this.newsPaging.currentPage) {
      shouldPerformGetNews = this.shouldPerformGetNews();
      this.router.navigate(['/news/page/', page.toString()]);
    }
    if (shouldPerformGetNews) {
      this.newsPaging = null;
      this.news = null;
      this.performGetNews(page)
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
