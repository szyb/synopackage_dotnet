import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { DeferLoadModule } from '@trademe/ng-defer-load';

import { AppComponent } from './app.component';
import { SourcesComponent } from './sources/sources.component';
import { HomeComponent } from './home/home.component';
import { WaitComponent } from './components/wait/wait.component';
import { UserSettingsComponent } from './components/user-settings/user-settings.component';
import { UserSettingsService } from './shared/user-settings.service';
import { DialogComponent } from './components/dialog/dialog.component';
import { UserSettingsDisplayComponent } from './components/user-settings-display/user-settings-display.component';
import { BrowseSourceComponent } from './browse-source/browse-source.component';
import { SearchComponent } from './search/search.component';
import { PackageInfoComponent } from './components/package-info/package-info.component';
import { ParametersComponent } from './components/parameters/parameters.component';
import { CreditsComponent } from './credits/credits.component';
import { ChangelogComponent } from './changelog/changelog.component';
import { DownloadDialogComponent } from './components/download-dialog/download-dialog.component';


export const routerConfig: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'sources',
    component: SourcesComponent,
    pathMatch: 'full'
  },
  {
    path: 'sources/:name',
    component: BrowseSourceComponent
  },
  {
    path: 'search',
    component: SearchComponent
  },
  {
    path: 'search/keyword/:keyword',
    component: SearchComponent
  },
  {
    path: 'search/keyword/:keyword/model/:model',
    component: SearchComponent
  },
  {
    path: 'search/keyword/:keyword/model/:model/version/:version',
    component: SearchComponent
  },
  {
    path: 'search/keyword/:keyword/model/:model/version/:version/channel/:channel',
    component: SearchComponent
  },
  {
    path: 'credits',
    component: CreditsComponent
  },
  {
    path: 'changelog',
    component: ChangelogComponent
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/home',
    pathMatch: 'full'
  }
];
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SourcesComponent,
    WaitComponent,
    UserSettingsComponent,
    UserSettingsDisplayComponent,
    BrowseSourceComponent,
    SearchComponent,
    PackageInfoComponent,
    ParametersComponent,
    DialogComponent,
    CreditsComponent,
    ChangelogComponent,
    DownloadDialogComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    HttpClientModule,
    NgSelectModule,
    FormsModule,
    DeferLoadModule,
    RouterModule.forRoot(routerConfig),
    MDBBootstrapModule.forRoot()
  ],
  providers: [UserSettingsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
