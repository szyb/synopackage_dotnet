using Microsoft.Extensions.Logging;
using synopackage_dotnet.Model.DTOs;
using System.Collections.Generic;

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

  }
}
