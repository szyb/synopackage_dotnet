using Newtonsoft.Json;
using Scriban;
using synopackage_dotnet.Generator.Entities;
using System.Collections.Generic;
using System.IO;

namespace synopackage_dotnet.Generator
{
  public class NewsGeneratorHandler : GeneratorHandlerAbstract
  {
    public override bool CanHandle(string filePath)
    {
      string fileName = Path.GetFileNameWithoutExtension(filePath);
      return fileName == "news";
    }

    public override string ConcreteHandle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<List<NewsDto>>(content);
      Template template = Template.Parse(GetFromResource("synopackage_dotnet.Generator.Templates.News.sbncs"));
      var rendered = template.Render(new
      {
        News = list
      });
      return rendered;
    }
  }
}
