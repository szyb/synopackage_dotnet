using Synopackage.Model.DTOs;
using System.Collections.Generic;

namespace Synopackage.Model.Services
{
  public interface ISourceService : IDomainService
  {
    SourcesDTO GetAllSources();
    bool ValidateSource(string name);
    SourceDTO GetSource(string name);
    IEnumerable<SourceDTO> GetAllActiveSources();
    IEnumerable<SourceDTO> GetActiveSources(bool? synopackageChoice, int? version);
  }
}
