import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-credits',
  templateUrl: './credits.component.html',
})

export class CreditsComponent implements OnInit {

  public libraries: any;

  constructor(private titleService: Title) {
    this.titleService.setTitle('Credits - synopackage.com');
  }

  ngOnInit(): void {
    this.libraries = [
      {
        name: 'MDB Bootstrap (Free)',
        description: 'Material Design for Bootstrap',
        projectUrl: 'https://mdbootstrap.com',
        licenseUrl: 'https://mdbootstrap.com/license/'
      },
      {
        name: 'RestSharp',
        description: 'Simple REST and HTTP API Client for .NET',
        projectUrl: 'http://restsharp.org',
        licenseUrl: 'https://raw.githubusercontent.com/restsharp/RestSharp/master/LICENSE.txt'
      },
      {
        name: 'Json.NET',
        description: 'Popular high-performance JSON framework for .NET',
        projectUrl: 'https://www.newtonsoft.com/json',
        licenseUrl: 'https://raw.githubusercontent.com/JamesNK/Newtonsoft.Json/master/LICENSE.md'
      },
      {
        name: 'Serilog',
        description: 'Simple .NET logging with fully-structured events',
        projectUrl: 'https://serilog.net',
        licenseUrl: 'https://raw.githubusercontent.com/serilog/serilog/dev/LICENSE'
      },
      {
        name: 'Autofac',
        description: 'Inversion of Control container for .NET Core, ASP.NET Core, .NET 4.5.1+, Universal Windows apps, and more.',
        projectUrl: 'https://autofac.org',
        licenseUrl: 'https://raw.githubusercontent.com/autofac/Autofac/develop/LICENSE'
      }
    ];
  }

}
