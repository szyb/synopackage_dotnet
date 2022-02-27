
export class RepositoryDetailsDTO {
  public url: string;
  public name: string;
  public description: string;
  public sources: string[];

}


export class RepositoryDTO {
  public details: RepositoryDetailsDTO[]
}
