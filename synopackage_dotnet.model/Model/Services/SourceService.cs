using Microsoft.Extensions.Logging;
using synopackage_dotnet.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace synopackage_dotnet.Model.Services
{
  public class SourceService : ISourceService
  {

    private readonly ILogger<SourceService> logger;

    public SourceService(ILogger<SourceService> logger)
    {
      this.logger = logger;
    }

    public IEnumerable<SourceDTO> GetAllActiveSources() => SourceHelper.ActiveSources;

    public SourcesDTO GetAllSources() => SourceHelper.GetAllSources();

    public SourceDTO GetSource(string name) => SourceHelper.GetSourceByName(name);

    public bool ValidateSource(string name) => SourceHelper.GetSourceByName(name) != null;

    public IEnumerable<SourceDTO> GetActiveSources(bool? synopackageChoice, int? version)
    {
      IEnumerable<SourceDTO> result = SourceHelper.ActiveSources.Where(p => !p.IsOfficial);
      if (synopackageChoice.HasValue && synopackageChoice.Value)
        result = result.Where(p => p.SynopackageChoice.HasValue && p.SynopackageChoice.Value);
      else if (synopackageChoice.HasValue && !synopackageChoice.Value)
        result = result.Where(p => !p.SynopackageChoice.HasValue || !p.SynopackageChoice.Value);

      if (version.HasValue)
        result = result.Where(p => (p.MinMajorVersion ?? 0) <= version.Value && (p.MaxMajorVersion ?? 999) >= version.Value);
      return result;
    }
  }
}
