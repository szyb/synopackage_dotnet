using Newtonsoft.Json;
using Scriban;
using synopackage_dotnet.Generator.Entities;
using System.Collections.Generic;
using System.IO;

namespace synopackage_dotnet.Generator
{
  public class SourcesGeneratorHandler : GeneratorHandlerAbstract
  {
    public override bool CanHandle(string filePath)
    {
      string fileName = Path.GetFileNameWithoutExtension(filePath);
      return fileName == "sources";
    }

    public override string ConcreteHandle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<IList<SourceDto>>(content);
      Template template = Template.Parse(GetFromResource("synopackage_dotnet.Generator.Templates.Sources.sbncs"));
      var rendered = template.Render(new
      {
        Sources = list
      });
      return rendered;
    }
  }
}
