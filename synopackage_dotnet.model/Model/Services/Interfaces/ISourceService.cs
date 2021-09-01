using synopackage_dotnet.Model.DTOs;
using System.Collections.Generic;

namespace synopackage_dotnet.Model.Services
{
  public interface ISourceService : IDomainService
  {
    SourcesDTO GetAllSources();
    bool ValidateSource(string name);
    SourceDTO GetSource(string name);
    IEnumerable<SourceDTO> GetAllActiveSources();
  }
}
