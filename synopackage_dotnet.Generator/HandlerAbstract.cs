namespace synopackage_dotnet.Generator
{
  public abstract class HandlerAbstract : IHandler
  {
    IHandler next = null;

    public abstract bool CanHandle(string filePath);
    public virtual string Handle(string filePath)
    {
      if (this.next != null)
      {
        return this.next.Handle(filePath);
      }
      else
      {
        return null;
      }
    }

    public IHandler SetupNext(IHandler next)
    {
      this.next = next;
      return this.next;
    }
  }
}
