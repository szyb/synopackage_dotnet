import { Component, ViewChild, OnInit } from '@angular/core';
import { UserSettingsComponent } from './components/user-settings/user-settings.component';
import { UserSettingsService } from './shared/user-settings.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
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

  loadScript() {
    if (environment.useGoogleAnalytics) {
      const node = document.createElement('script');
      node.src = `https://www.googletagmanager.com/gtag/js?id=` + environment.googleAnalyticsCode;
      node.type = 'text/javascript';
      node.async = true;
      node.charset = 'utf-8';
      document.getElementsByTagName('head')[0].appendChild(node);

      const node2 = document.createElement('script');
      node2.text = `window.dataLayer = window.dataLayer || [];
      function gtag(){dataLayer.push(arguments);}
      gtag('js', new Date());

      gtag('config', '` + environment.googleAnalyticsCode + `');
      `;
      document.getElementsByTagName('head')[0].appendChild(node2);
    }

  }

  ngOnInit() {
    this.loadScript();
  }

  isSetup(): boolean {
    return this.userSettingsService.isSetup();
  }
}
