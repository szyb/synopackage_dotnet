import { PackageDTO, SourceServerResponseDTO } from '../sources/sources.model';

export class SearchResultDTO {
  name: string;
  isSearchEnded: boolean;
  isValid: boolean;
  errorMessage: string;
  count: number;
  packages: PackageDTO[];
  response: SourceServerResponseDTO;
}
