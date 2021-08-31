using System.IO;
using System.Reflection;

namespace synopackage_dotnet.Generator
{
  public abstract class GeneratorHandlerAbstract : IGeneratorHandler
  {
    IGeneratorHandler next = null;

    public abstract bool CanHandle(string filePath);
    public abstract string ConcreteHandle(string filePath);
    public string Handle(string filePath)
    {
      if (CanHandle(filePath))
        return this.ConcreteHandle(filePath);
      if (!CanHandle(filePath) && this.next != null)
        return this.next.Handle(filePath);

      return null;
    }

    public IGeneratorHandler SetupNext(IGeneratorHandler next)
    {
      this.next = next;
      return this.next;
    }

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
