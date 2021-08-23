namespace synopackage_dotnet.Generator
{
  public interface IHandler
  {
    IHandler SetupNext(IHandler next);
    string Handle(string filePath);
    bool CanHandle(string filePath);
  }
}
