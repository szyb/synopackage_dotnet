import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NewsService } from '../shared/news.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent implements OnInit {


  constructor(private titleService: Title) {
    this.titleService.setTitle('Home - synopackage.com');
  }

  ngOnInit(): void {
  }

}
