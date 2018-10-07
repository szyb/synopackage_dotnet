import { Component, ViewChild } from '@angular/core';
import { UserSettingsComponent } from './components/user-settings/user-settings.component';
import { UserSettingsService } from './shared/user-settings.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'synopackage.com';
  public actualYear: string;

  constructor(private userSettingsService: UserSettingsService) {
    const date = new Date();
    this.actualYear = date.getFullYear().toString();

  }

  @ViewChild(UserSettingsComponent) basicModal: UserSettingsComponent;
  showUserSettingsModal() {
    this.basicModal.showModal();
  }

  isSetup(): boolean {
    return this.userSettingsService.isSetup();
  }
}
