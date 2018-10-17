
export class SourceDTO {
  name: string;
  url: string;
  www: string;
  disabledReason: string;
}

export class SourcesDTO {
  activeSources: SourceDTO[];
  inActiveSources: SourceDTO[];
}

export class SourceLiteDTO {
  name: string;
}

export class PackageDTO {
  name: string;
  thumbnailUrl: string;
  version: string;
  package: string;
  description: string;
  isBeta: boolean;
  downloadLink: string;
  iconFileName: string;
}

export class SourceServerResponseDTO {
  result: boolean;
  errorMessage: string;
  packages: PackageDTO[];
}

