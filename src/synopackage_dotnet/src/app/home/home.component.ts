import { Component, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { UserSettingsComponent } from '../components/user-settings/user-settings.component';
import { UserSettingsService } from '../shared/user-settings.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['home.component.css']
})

export class HomeComponent implements OnInit {

  public keyword: string;

  constructor(private userSettingsService: UserSettingsService,
    private titleService: Title,
    private router: Router) {
    this.titleService.setTitle('Home - synopackage.com');
  }

  @ViewChild(UserSettingsComponent) basicModal: UserSettingsComponent;
  showUserSettingsModal() {
    this.basicModal.showModal();
  }

  ngOnInit(): void {
  }

  onSearchButton() {
    this.router.navigate(['/search/keyword', this.keyword]);
  }

  onSourcesButton() {
    this.router.navigate(['/sources']);
  }

  onBrowseButton() {
    this.router.navigate(['/sources/synocommunity']);
  }

  onSettingsButton() {
    this.showUserSettingsModal();
  }

  onEnter() {
    this.onSearchButton();
  }
}
