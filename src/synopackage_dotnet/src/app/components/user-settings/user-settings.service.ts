export class UserSettingsService {
    public saveUserSettings = (version: string, model: string, isBeta: boolean) => {
        localStorage.setItem('version', version);
        localStorage.setItem('model', model);
        localStorage.setItem('isBeta', isBeta.toString());
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
