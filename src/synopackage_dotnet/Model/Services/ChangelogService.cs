using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class ChangelogService : IChangelogService
  {
    private readonly string configFile = "Config/changelog.json";

    private IEnumerable<ChangelogDTO> GetChangelogsInternal()
    {
      var changelogJson = File.ReadAllText(configFile);
      return JsonConvert.DeserializeObject<ChangelogDTO[]>(changelogJson);

    }

    public IEnumerable<ChangelogDTO> GetChangelogs()
    {
      return GetChangelogsInternal();
    }
  }
}