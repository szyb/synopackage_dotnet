using Newtonsoft.Json;
using Scriban;
using Synopackage.Generator.Entities;
using System.Collections.Generic;
using System.IO;

namespace Synopackage.Generator.ConfigGenerator.Handlers
{
  public class NewsGeneratorHandler : GeneratorHandlerAbstract
  {
    public override string Handle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<List<NewsDto>>(content);
      Template template = Template.Parse(GetFromResource("Synopackage.Generator.ConfigGenerator.Templates.News.sbncs"));
      var rendered = template.Render(new
      {
        News = list
      });
      return rendered;
    }
  }
}
