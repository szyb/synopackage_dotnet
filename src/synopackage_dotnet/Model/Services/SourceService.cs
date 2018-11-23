using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
      return GetAllSourcesInternal().Where(p => p.Name == source).Any() ? true : false;
    }

    public SourceDTO GetSource(string source)
    {
      return GetAllSourcesInternal().Where(p => p.Name == source).FirstOrDefault();
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
      return result;
    }

    public IEnumerable<SourceDTO> GetAllActiveSources()
    {
      var result = GetAllSourcesInternal()
        .Where(item => item.Active);

      return result;
    }

    public void DownloadRequest(DownloadRequestDTO downloadRequest)
    {
      if (!this.ValidateSource(downloadRequest.SourceName))
      {
        logger.LogError($"Coudn't not find source '{downloadRequest.SourceName}'");
        return;
      }

      var message = $"Download request: '{downloadRequest.PackageName}' from '{downloadRequest.SourceName}'. Link: {downloadRequest.RequestUrl}";
      logger.LogInformation(message);
    }
  }
}
