using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace synopackage_dotnet.Model.Services
{
  public class SourceService : ISourceService
  {
    private SourceDTO[] GetAllSourcesInternal()
    {
      var sourcesJson = File.ReadAllText("Config/sources.json");
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
      return GetAllSourcesInternal().Where(p => p.Name == source) == null ? false : true;
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
  }
}
