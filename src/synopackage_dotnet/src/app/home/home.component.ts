import { Component, OnInit } from '@angular/core';
import { NewsDTO } from '../shared/model';
import { NewsService } from '../shared/news.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent implements OnInit {

  private news: NewsDTO;

  constructor(private newsService: NewsService) {
  }
  ngOnInit(): void {
    this.newsService.getNews()
      .subscribe(val => {
        this.news = val;
      });
  }

}
