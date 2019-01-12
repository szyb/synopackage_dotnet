import { Component, OnInit } from '@angular/core';
import { ChangelogDTO } from '../shared/model';
import { Title } from '@angular/platform-browser';


@Component({
  selector: 'app-privacy-policy',
  templateUrl: './privacy-policy.component.html',
})

export class PrivacyPolicyComponent implements OnInit {

  public privacyPolicies: any;


  constructor(private titleService: Title) {
    this.titleService.setTitle('Privacy policy - synopackage.com');
  }

  ngOnInit(): void {
    this.privacyPolicies = [
      {
        title: 'We use local storage & cookies',
        // tslint:disable-next-line:max-line-length
        description: 'Nothing unusual. By continuing to browse the site, you are agreeing to our use of local storage & cookies. Local storage is used to save your preferences i.e. your device model and DSM version. This helps you not to select your preferences every time you enter the page. Cookies may be used by third party libraries and services'
      },
      {
        title: 'We also store query logs',
        // tslint:disable-next-line:max-line-length
        description: 'We do want to know what devices are in use. Those logs are stored anonymously, so <b>we do not store your IP nor any other your personal information</b>. However your IP might be collected by the hosting web server, which is outside our jurisdiction.'
      },
      {
        title: 'We also use statistics',
        // tslint:disable-next-line:max-line-length
        description: 'Currently we use Google analytics. Their privacy policy is located at link below',
        link: 'https://support.google.com/analytics/answer/6004245',
        linkDescription: 'Google analytics privacy policy'
      }
    ];
  }

}
