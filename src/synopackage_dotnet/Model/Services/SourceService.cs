using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
    public class SourceService : ISourceService
    {
        public IEnumerable<SourceDTO> GetList()
        {
            var sourcesJson = File.ReadAllText("Config/sources.json");
            var sources = JsonConvert.DeserializeObject<SourceDTO[]>(sourcesJson);
            return sources;
        }
    }
}
