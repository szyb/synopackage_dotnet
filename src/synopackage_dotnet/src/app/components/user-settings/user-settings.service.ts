export class UserSettingsService {
    public saveUserSettings = (version: string, model: string) => {
        localStorage.setItem('version', version);
        localStorage.setItem('model', model);
    }

    public getUserVersion(): string {
        return localStorage.getItem('version');
    }

    public getUserModel(): string {
        return localStorage.getItem('model');
    }

    public isSetup(): boolean {
        const v = localStorage.getItem('version');
        const m = localStorage.getItem('model');
        if (v === null || m === null)
            return false;
        else
            return true;
    }
}
