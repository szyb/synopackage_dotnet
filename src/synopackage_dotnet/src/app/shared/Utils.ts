
export class Utils {

  public static getQueryParams(params: any): string {
    let paramsQuery = '';

    for (const key in params) {
      if (params.hasOwnProperty(key)) {
        const value =  params[key];
        paramsQuery += params[key] != null && `&${key}=${encodeURIComponent(value)}` || '';
      }
    }

    if (paramsQuery.startsWith('&')) {
      paramsQuery = '?' + paramsQuery.slice(1);
    }
    console.log(paramsQuery);

    return paramsQuery;
  }
}
