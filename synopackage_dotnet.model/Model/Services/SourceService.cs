using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace synopackage_dotnet.Model.Services
{
  public class SourceService : ISourceService
  {
    private readonly string configFile = "Config/sources.json";

    private readonly ILogger<SourceService> logger;

    public SourceService(ILogger<SourceService> logger)
    {
      this.logger = logger;
    }
    private SourceDTO[] GetAllSourcesInternal()
    {
      var sourcesJson = File.ReadAllText(configFile);
      var sources = JsonConvert.DeserializeObject<SourceDTO[]>(sourcesJson);
      Parallel.ForEach(sources, item =>
      {
        if (string.IsNullOrWhiteSpace(item.Www))
          item.Www = item.Url;
        if (!item.IsOfficial)
          item.DisplayUrl = item.Url;
        else
          item.DisplayUrl = "Synology's Official Package Center";
      });
      return sources;
    }

    public SourcesDTO GetAllSources()
    {
      return PrepareSources(GetAllSourcesInternal());
    }

    public bool ValidateSource(string source)
    {
      if (source == null)
        return false;
      return GetAllSourcesInternal().Any(p => p.Name == source);
    }

    public SourceDTO GetSource(string source)
    {
      return GetAllSourcesInternal().FirstOrDefault(p => p.Name == source);
    }

    private SourcesDTO PrepareSources(SourceDTO[] sources)
    {
      SourcesDTO result = new SourcesDTO();
      foreach (var source in sources)
      {
        if (source.Active)
        {
          result.ActiveSources.Add(source);
        }
        else
        {
          result.InActiveSources.Add(source);
        }
      }
      FileInfo fi = new FileInfo(configFile);
      result.LastUpdateDate = fi.LastWriteTimeUtc;
      return result;
    }

    public IEnumerable<SourceDTO> GetAllActiveSources()
    {
      var result = GetAllSourcesInternal()
        .Where(item => item.Active);

      return result;
    }
  }
}
