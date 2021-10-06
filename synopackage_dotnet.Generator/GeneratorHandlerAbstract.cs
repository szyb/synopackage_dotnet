using System.IO;
using System.Reflection;

namespace synopackage_dotnet.Generator
{
  public abstract class GeneratorHandlerAbstract : IGeneratorHandler
  {
    public abstract string Handle(string filePath);

    private static readonly Assembly assembly = typeof(GeneratorHandlerAbstract).Assembly;
    protected string GetFromResource(string name)
    {
      using (Stream stream = assembly.GetManifestResourceStream(name))
      using (StreamReader reader = new StreamReader(stream))
      {
        string result = reader.ReadToEnd();
        return result;
      }
    }
  }
}
