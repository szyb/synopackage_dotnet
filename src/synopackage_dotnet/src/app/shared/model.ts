
export class ParametersDTO {
  sourceName: string;
  model: string;
  version: string;
  isBeta: boolean;
  keyword: string;
  public channel(): string {
    if (this.isBeta) {
      return 'Beta';
    } else {
      return 'Stable';
    }
  }
}
