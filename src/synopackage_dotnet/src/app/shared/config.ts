import { environment } from '../../environments/environment';

export class Config {
  static baseUrl = environment.restBaseUrl;
  static apiUrl = Config.baseUrl + 'api/';
  static title = 'Synopackage';
  static defaultVersion = '"6.2-23739';
  static defaultModel = 'DS218Play';
}
