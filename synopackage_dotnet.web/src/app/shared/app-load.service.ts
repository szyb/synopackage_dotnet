import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { Config } from './config';

@Injectable()
export class AppLoadService {

  constructor() { }

  initializeApp(): Promise<any> {
    return new Promise<void>((resolve, reject) => {

      setTimeout(() => {
        if (environment.restBaseUrl === null) {
          let url = document.location.protocol.concat('//', document.location.hostname);

          if ((document.location.protocol === 'http:' && document.location.port !== '80') ||
            (document.location.protocol === 'https:' && document.location.port !== '443')) {
            url = url.concat(':', document.location.port);
          }
          url = url.concat('/');
          environment.restBaseUrl = url;
          Config.apiUrl = environment.restBaseUrl.concat('api/');
          Config.baseUrl = url;
          console.log('API Url is set');
        }
        resolve();
      }, 3000);
    });
  }
}
