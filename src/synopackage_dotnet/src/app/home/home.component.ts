import { Component, OnInit } from '@angular/core';
import { NewsDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';
import { NewsService } from '../shared/news.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent implements OnInit {

  private news: NewsDTO;


  constructor(private titleService: Title, private newsService: NewsService) {
    this.titleService.setTitle('Home - synopackage.com');
  }

  ngOnInit(): void {
    this.newsService.getNews()
      .subscribe(val => {
        this.news = val;
      });
  }

}
