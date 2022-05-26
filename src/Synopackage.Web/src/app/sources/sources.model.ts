import { ParametersDTO } from '../shared/model';

export class SourceDTO {
  name: string;
  url: string;
  www: string;
  disabledReason: string;
  displayUrl: string;
  isOfficial: boolean;
  isHealthy: boolean;
  healthCheckDescription: string;
  info: string;
  isDownloadDisabled: boolean;
}

export class SourcesDTO {
  activeSources: SourceDTO[];
  inactiveSources: SourceDTO[];
  lastUpdateDate: Date;
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
  sourceName: string;
  showImage = false;
}

export class SourceServerResponseDTO {
  result: boolean;
  errorMessage: string;
  parameters: ParametersDTO;
  packages: PackageDTO[];
  resultFrom: number;
  cacheOld: number;
  sourceInfo: string;
  isDownloadDisabled: boolean;
}
