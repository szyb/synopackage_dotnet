import { Subject } from 'rxjs';
import { Injectable, OnInit } from '@angular/core';
import { Utils } from './Utils';

@Injectable({
  providedIn: 'root',
})
export class UserSettingsService implements OnInit {

  protected subject = new Subject();
  public events = this.subject.asObservable();

  constructor() {
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

  public getUserDisplayVersion(): string {
    const ver = localStorage.getItem('version');
    if (!Utils.isNullOrWhitespace(ver)) {
      if (ver.indexOf('-') >= 0) {
        return ver.substr(0, ver.indexOf('-'));
      } else {
        return ver;
      }
    } else {
      return null;
    }
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
