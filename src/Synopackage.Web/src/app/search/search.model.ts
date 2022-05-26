import { PackageDTO, SourceServerResponseDTO } from '../sources/sources.model';

export class SearchResultDTO {
  name: string;
  url: string;
  www: string;
  isSearchEnded: boolean;
  isValid: boolean;
  isExpiredCache: boolean;
  cacheOldString: string;
  errorMessage: string;
  count: number;
  noPackages: boolean;
  packages: PackageDTO[];
  response: SourceServerResponseDTO;
  isCollapsed: boolean;
  sourceInfo: string;
  isDownloadDisabled: boolean;
}
