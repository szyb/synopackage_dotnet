import { Subject } from 'rxjs';
import { Injectable, OnInit } from '@angular/core';
import { ModelsService } from './models.service';
import { VersionsService } from './versions.service';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UserSettingsService implements OnInit {

  protected subject = new Subject();
  public events = this.subject.asObservable();

  constructor(private modelsService: ModelsService,
    private versionsService: VersionsService) {
  }

  ngOnInit() {

  }
  public saveUserSettings = (version: string, model: string, isBeta: boolean) => {
    localStorage.setItem('version', version);
    localStorage.setItem('model', model);
    localStorage.setItem('isBeta', isBeta.toString());
    this.subject.next();
    // this.subject.complete();
  }

  dispathEvent(event) {
    this.subject.next(event);
  }

  public getUserVersion(): string {
    return localStorage.getItem('version');
  }

  public getUserModel(): string {
    return localStorage.getItem('model');
  }

  public getUserIsBeta(): boolean {
    const isBeta = localStorage.getItem('isBeta');
    if (isBeta === 'true') {
      return true;
    } else {
      return false;
    }
  }

  public isSetup(): boolean {
    return this.isModelSet() && this.isVersionSet();
  }

  private isModelSet(): boolean {
    const m = localStorage.getItem('model');
    if (m === null) {
      return false;
    } else {
      return true;
    }
  }

  private isVersionSet(): boolean {
    const v = localStorage.getItem('version');
    if (v === null) {
      return false;
    } else {
      return true;
    }
  }

  private isBetaSet(): boolean {
    const b = localStorage.getItem('isBeta');
    if (b === null) {
      return false;
    } else {
      return true;
    }
  }
}
