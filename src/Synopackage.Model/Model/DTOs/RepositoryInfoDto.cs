using System.Collections.Generic;

namespace synopackage_dotnet.model.Model.DTOs
{
  public class RepositoryInfoDto
  {
    public IList<RepositoryInfoDetailsDto> Details { get; set; } = new List<RepositoryInfoDetailsDto>();
  }
}