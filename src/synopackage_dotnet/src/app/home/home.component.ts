import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['home.component.css']
})

export class HomeComponent implements OnInit {

  public keyword: string;

  constructor(private titleService: Title,
    private router: Router) {
    this.titleService.setTitle('Home - synopackage.com');
  }

  ngOnInit(): void {
  }

  onSearchButton() {
    this.router.navigate(['/search/keyword', this.keyword]);
  }

  onBrowseButton() {
    this.router.navigate(['/sources']);
  }

  onSettingsButton() {
    console.log('TODO');
  }
}
