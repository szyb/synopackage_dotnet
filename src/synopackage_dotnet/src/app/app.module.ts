import { BrowserModule } from '@angular/platform-browser';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
// import { NgxMapboxGLModule } from 'ngx-mapbox-gl';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { SourcesComponent } from './sources/sources.component';
import { HomeComponent } from './home/home.component';
import { WaitComponent } from './components/wait/wait.component';
import { UserSettingsComponent } from './components/user-settings/user-settings.component';
import { UserSettingsService } from './shared/user-settings.service';
import { DialogComponent } from './components/dialog/dialog.component';
import { UserSettingsDisplayComponent } from './components/user-settings-display/user-settings-display.component';
import { BrowseSourceComponent } from './browse-source/browse-source.component';


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
    DialogComponent
    // DisplayMapComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    HttpClientModule,
    NgSelectModule,
    FormsModule,
    RouterModule.forRoot(routerConfig),
    MDBBootstrapModule.forRoot()
  ],
  providers: [UserSettingsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
