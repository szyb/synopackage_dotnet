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

    public IEnumerable<SourceLiteDTO> GetAllActiveSources()
    {
      List<SourceLiteDTO> list = new List<SourceLiteDTO>();
      var sourcesJson = File.ReadAllText("Config/sources.json");
      var sources = JsonConvert.DeserializeObject<SourceDTO[]>(sourcesJson);

      var result = sources
        .Where(item => item.Active)
        .ToList()
        .ConvertAll<SourceLiteDTO>(delegate (SourceDTO s)
        {
          SourceLiteDTO sl = new SourceLiteDTO();
          sl.Name = s.Name;
          return new SourceLiteDTO() { Name = s.Name };
        });

      return result;
    }
  }
}
