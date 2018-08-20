export class UserSettingsService {
    public saveUserSettings = (version: string, model: string) => {
        // localStorage.clear();
        localStorage.setItem('version', version);
        localStorage.setItem('model', model);
    }

    public getUserVersion(): string {
        return localStorage.getItem('version');
    }

    public getUserModel(): string {
        return localStorage.getItem('model');
    }
}
