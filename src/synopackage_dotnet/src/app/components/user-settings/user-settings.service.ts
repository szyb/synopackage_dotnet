import {  Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
  })
export class UserSettingsService {

    protected subject = new Subject();
    public events = this.subject.asObservable();

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
        const v = localStorage.getItem('version');
        const m = localStorage.getItem('model');
        if (v === null || m === null) {
            return false;
        } else {
            return true;
        }
    }
}
