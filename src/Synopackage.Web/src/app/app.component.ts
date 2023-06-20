import { Component, ViewChild, OnInit } from '@angular/core';
import { UserSettingsComponent } from './components/user-settings/user-settings.component';
import { UserSettingsService } from './shared/user-settings.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'synopackage.com';
  public actualYear: string;
  public version: string;

  constructor(private userSettingsService: UserSettingsService) {
    const date = new Date();
    this.actualYear = date.getFullYear().toString();
  }

  @ViewChild(UserSettingsComponent, { static: true }) basicModal: UserSettingsComponent;
  showUserSettingsModal() {
    this.basicModal.showModal();
  }

  loadScript() {
    if (environment.useGoogleAnalytics) {
      const node = document.createElement('script');
      node.src = `https://www.google-analytics.com/analytics.js`;
      node.type = 'text/javascript';
      node.async = true;
      document.getElementsByTagName('head')[0].appendChild(node);

      const node3 = document.createElement('script');
      node3.src = `assets/autotrack.js`;
      node3.type = 'text/javascript';
      node3.async = true;
      document.getElementsByTagName('head')[0].appendChild(node3);

      const node2 = document.createElement('script');
      node2.text = `window.ga=window.ga||function(){(ga.q=ga.q||[]).push(arguments)};ga.l=+new Date;
      ga('create', '` + environment.googleAnalyticsCode + `', 'auto');
      // Replace the following lines with the plugins you want to use.
      ga('require', 'eventTracker');
      ga('require', 'outboundLinkTracker');
      ga('require', 'urlChangeTracker');
      ga('send', 'pageview');
      `;
      document.getElementsByTagName('head')[0].appendChild(node2);
    }
    if (environment.useGoogleAdSense) {
      const node5 = document.createElement('script');
      node5.src = `https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=` + environment.googleAdSense;
      node5.type = 'text/javascript';
      node5.async = true;
      node5.crossOrigin = "anonymous";
      document.getElementsByTagName('head')[0].appendChild(node5);
    }
    if (environment.useGTagAnalytics) {
      const node6 = document.createElement('script');
      node6.src = `https://www.googletagmanager.com/gtag/js?id=` + environment.gtagCode;
      node6.type = 'text/javascript';
      node6.async = true;
      document.getElementsByTagName('head')[0].appendChild(node6);

      const node7 = document.createElement('script');
      node7.text = `window.dataLayer = window.dataLayer || [];
      function gtag(){dataLayer.push(arguments);}
      gtag('js', new Date());

      gtag('config', '`+ environment.gtagCode + `');
      `;
      node7.type = 'text/javascript';
      document.getElementsByTagName('head')[0].appendChild(node7);
    }

    const node4 = document.createElement('script');
    node4.src = `https://cdnjs.buymeacoffee.com/1.0.0/widget.prod.min.js`;
    node4.setAttribute('data-name', 'BMC-Widget');
    node4.setAttribute('data-cfasync', 'false');
    node4.setAttribute('data-id', 'synopackage');
    node4.setAttribute('data-description', 'Support me on Buy me a coffee!');
    node4.setAttribute('data-message', 'Support me on Buy me a coffee!');
    node4.setAttribute('data-color', '#5F7FFF');
    node4.setAttribute('data-position', 'Right');
    node4.setAttribute('data-x_margin', '18');
    node4.setAttribute('data-y_margin', '18');
    node4.async = true;
    document.getElementsByTagName('head')[0].appendChild(node4);
    node4.onload = function () {
      var evt = document.createEvent('Event');
      evt.initEvent('DOMContentLoaded', false, false);
      window.dispatchEvent(evt);
    }


  }

  ngOnInit() {
    this.version = environment.version;
    this.loadScript();
  }

  isSetup(): boolean {
    return this.userSettingsService.isSetup();
  }
}
