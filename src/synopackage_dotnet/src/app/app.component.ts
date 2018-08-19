import { Component, ViewChild } from '@angular/core';
import { UserSettingsComponent } from './components/user-settings/user-settings.component';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'synopackage.com';

  @ViewChild(UserSettingsComponent)  basicModal : UserSettingsComponent;
  showUserSettingsModal() {
    this.basicModal.showModal();
  }
}
