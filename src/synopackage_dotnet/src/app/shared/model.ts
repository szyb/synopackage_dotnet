
export class ParametersDTO {
  sourceName: string;
  model: string;
  version: string;
  isBeta: boolean;
  keyword: string;
  isSearch: boolean;
  public channel(): string {
    if (this.isBeta) {
      return 'Beta';
    } else {
      return 'Stable';
    }
  }
}

export class NewsDTO {
  public date: Date;
  public title: string;
  public body: string[];
  public routerLinkDescription: string;
  public routerLink: string;
  public externalLinkDescription: string;
  public externalLink: string;
}

export class ChangelogDTO {
  public version: string;
  public date: Date;
  public new: string[];
  public improved: string[];
  public fixed: string[];
  public removedSources: string;
}
