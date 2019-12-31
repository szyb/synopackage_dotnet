import { Component, OnInit } from '@angular/core';
import { ChangelogDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';
import { ChangelogService } from '../shared/changelog.service';

@Component({
  selector: 'app-changelog',
  templateUrl: './changelog.component.html',
})

export class ChangelogComponent implements OnInit {

  public changelogs: ChangelogDTO[];


  constructor(private titleService: Title, private changelogService: ChangelogService) {
    this.titleService.setTitle('Changelog - synopackage.com');
  }

  ngOnInit(): void {
    this.changelogService.getChangelogs()
      .subscribe(val => {
        this.changelogs = val;
      });
  }

}
