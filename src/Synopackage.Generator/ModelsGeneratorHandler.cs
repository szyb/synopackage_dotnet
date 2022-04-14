using Newtonsoft.Json;
using Scriban;
using synopackage_dotnet.Generator.Entities;
using System.Collections.Generic;
using System.IO;

namespace synopackage_dotnet.Generator
{
  public class ModelsGeneratorHandler : GeneratorHandlerAbstract
  {

    public override string Handle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<List<ModelDto>>(content);
      list.Sort((x, y) => { return x.Name.CompareTo(y.Name); });
      Template template = Template.Parse(GetFromResource("Synopackage.Generator.Templates.Models.sbncs"));
      var rendered = template.Render(new
      {
        Models = list
      });
      return rendered;
    }
  }
}
