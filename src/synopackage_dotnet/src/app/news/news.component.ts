import { Component, OnInit } from '@angular/core';
import { NewsDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';
import { NewsService } from '../shared/news.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
})

export class NewsComponent implements OnInit {

  public news: NewsDTO;


  constructor(private titleService: Title, private newsService: NewsService) {
    this.titleService.setTitle('News - synopackage.com');
  }

  ngOnInit(): void {
    this.newsService.getNews()
      .subscribe(val => {
        this.news = val;
      });
  }

}
