export class UserSettingsService {
    public saveUserSettings = (version: string, model: string) =>
    {
        // localStorage.clear();
        localStorage.setItem('version', version);
        localStorage.setItem('model', model);
        for (var a in localStorage) {
            console.log(a, ' = ', localStorage[a]);
         }      
    }
    
    public getUserVersion() : string
    {
        return localStorage.getItem('version');
    }

    public getUserModel() : string
    {
        return localStorage.getItem('model');
    }
}
