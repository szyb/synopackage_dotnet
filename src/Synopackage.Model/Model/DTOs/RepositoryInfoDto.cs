using System.Collections.Generic;

namespace Synopackage.model.Model.DTOs
{
  public class RepositoryInfoDto
  {
    public IList<RepositoryInfoDetailsDto> Details { get; set; } = new List<RepositoryInfoDetailsDto>();
  }
}