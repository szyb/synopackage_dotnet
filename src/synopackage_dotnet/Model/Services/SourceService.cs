using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using System.Linq;
using System;

namespace synopackage_dotnet.Model.Services
{
  public class SourceService : ISourceService
  {
    // public IEnumerable<SourceDTO> GetList()
    // {
    //   var sourcesJson = File.ReadAllText("Config/sources.json");
    //   var sources = JsonConvert.DeserializeObject<SourceDTO[]>(sourcesJson);
    //   return sources;
    // }

    public SourcesDTO GetAllSources()
    {
      var sourcesJson = File.ReadAllText("Config/sources.json");
      var sources = JsonConvert.DeserializeObject<SourceDTO[]>(sourcesJson);
      return PrepareSources(sources);
    }

    public bool ValidateSource(string source)
    {
      var sourcesJson = File.ReadAllText("Config/sources.json");
      var sources = JsonConvert.DeserializeObject<SourceDTO[]>(sourcesJson);

      return sources.Where(p => p.Name == source) == null ? false : true;
    }

    public SourceDTO GetSource(string source)
    {
      var sourcesJson = File.ReadAllText("Config/sources.json");
      var sources = JsonConvert.DeserializeObject<SourceDTO[]>(sourcesJson);

      return sources.Where(p => p.Name == source).FirstOrDefault();
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
  }
}
