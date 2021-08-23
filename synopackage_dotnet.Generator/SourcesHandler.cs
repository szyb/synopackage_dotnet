using Newtonsoft.Json;
using System.IO;

namespace synopackage_dotnet.Generator
{
  public class SourcesHandler : HandlerAbstract
  {
    public override bool CanHandle(string filePath)
    {
      string fileName = Path.GetFileNameWithoutExtension(filePath);
      return fileName == "sources";
    }

    public override string Handle(string filePath)
    {
      //string fileName = Path.GetFileNameWithoutExtension(filePath);
      var content = File.ReadAllText(filePath);
      dynamic obj = JsonConvert.DeserializeObject(content);
      return null;
    }
  }
}
