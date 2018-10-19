using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class NewsService : INewsService
  {
    public IEnumerable<NewsDTO> GetNews()
    {
      var newsJson = File.ReadAllText("Config/news.json");
      var news = JsonConvert.DeserializeObject<NewsDTO[]>(newsJson);
      return news;
    }
  }
}