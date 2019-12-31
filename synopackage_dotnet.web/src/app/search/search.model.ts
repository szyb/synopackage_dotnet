import { PackageDTO, SourceServerResponseDTO } from '../sources/sources.model';

export class SearchResultDTO {
  name: string;
  url: string;
  www: string;
  isSearchEnded: boolean;
  isValid: boolean;
  errorMessage: string;
  count: number;
  noPackages: boolean;
  packages: PackageDTO[];
  response: SourceServerResponseDTO;
  isCollapsed: boolean;
}
