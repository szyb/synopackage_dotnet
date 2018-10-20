import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(private titleService: Title) {
    this.titleService.setTitle('Home - synopackage.com');
  }
}
