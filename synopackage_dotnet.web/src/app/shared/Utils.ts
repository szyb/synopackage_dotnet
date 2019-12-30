
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
}
