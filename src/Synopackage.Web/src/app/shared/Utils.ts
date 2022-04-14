
export class Utils {

  public static getQueryParams(params: any): string {
    let paramsQuery = '';

    for (const key in params) {
      if (params.hasOwnProperty(key)) {
        const value = params[key];
        paramsQuery += params[key] != null && `&${key}=${encodeURIComponent(value)}` || '';
      }
    }

    if (paramsQuery.startsWith('&')) {
      paramsQuery = '?' + paramsQuery.slice(1);
    }

    return paramsQuery;
  }

  public static isNullOrWhitespace(input: string): boolean {
    return !input || !input.trim();
  }

  public static getCacheOldString(input: number): string {
    if (input <= 60) {
      return (Math.floor(input)).toString() + " second(s) ago";
    } else if (input <= 3600) {
      return (Math.floor(input / 60)).toString() + " minute(s) ago";
    } else if (input <= 86400) {
      return (Math.floor(input / 3600)).toString() + " hour(s) ago";
    } else {
      return (Math.floor(input / 86400)).toString() + " day(s) ago";
    }
  }
}
