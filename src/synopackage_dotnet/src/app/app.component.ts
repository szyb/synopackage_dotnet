import { Component, ViewChild } from '@angular/core';
import { UserSettingsComponent } from './components/user-settings/user-settings.component';
import { UserSettingsService } from './components/user-settings/user-settings.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'synopackage.com';

  constructor(private userSettingsService: UserSettingsService) {

  }

  @ViewChild(UserSettingsComponent)  basicModal: UserSettingsComponent;
  showUserSettingsModal() {
    this.basicModal.showModal();
  }

  isSetup(): boolean {
    return this.userSettingsService.isSetup();
  }
}
