namespace synopackage_dotnet.Generator
{
  public interface IGeneratorHandler
  {
    IGeneratorHandler SetupNext(IGeneratorHandler next);
    string Handle(string filePath);
    string ConcreteHandle(string filePath);
    bool CanHandle(string filePath);
  }
}
