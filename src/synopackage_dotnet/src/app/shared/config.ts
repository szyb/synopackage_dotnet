import { environment } from '../../environments/environment';

export class Config {
  static baseUrl = environment.restBaseUrl;
  static apiUrl = Config.baseUrl + 'api/';
  static title = 'Synopackage';
}
