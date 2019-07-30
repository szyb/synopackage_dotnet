import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';


@Component({
  selector: 'app-info',
  templateUrl: './info.component.html',
  styleUrls: ['info.component.scss']
})

export class InfoComponent {
  constructor(private titleService: Title) {
    this.titleService.setTitle('Info - synopackage.com');
  }
}
