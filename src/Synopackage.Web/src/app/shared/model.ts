
export class ParametersDTO {
  sourceName: string;
  model: string;
  modelOrUnique: string;
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

export class PagingParamsDTO {
  public page: number;
  public itemsPerPage: number;
}

export class PagingDTO {
  public totalPages: number;
  public currentPage: string;
  public itemsPerPage: number;
}

export class NewsPagingDTO extends PagingDTO {
  public items: NewsDTO[];
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

export class ChangelogPagingDTO extends PagingDTO {
  public items: ChangelogDTO[];
}

export class ChangelogDTO {
  public version: string;
  public date: Date;
  public new: string[];
  public improved: string[];
  public fixed: string[];
  public removedSources: string;
}

export class DownloadRequestDTO {
  public requestUrl: string;
  public sourceName: string;
  public packageName: string;
}
