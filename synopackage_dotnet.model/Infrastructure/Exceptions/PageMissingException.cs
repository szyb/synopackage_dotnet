using System;

namespace synopackage_dotnet
{
  public class PageMissingException : Exception
  {
    public PageMissingException(string message) : base(message)
    {

    }
  }
}
