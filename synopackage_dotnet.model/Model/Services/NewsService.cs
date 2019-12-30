using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class NewsService : INewsService
  {
    private readonly string configFile = "Config/news.json";

    private IEnumerable<NewsDTO> GetNewsInternal()
    {
      var newsJson = File.ReadAllText(configFile);
      return JsonConvert.DeserializeObject<NewsDTO[]>(newsJson);

    }
    public IEnumerable<NewsDTO> GetNews()
    {
      return GetNewsInternal();
    }
  }
}