using System;
using System.Runtime.Serialization;

namespace synopackage_dotnet
{
  [Serializable]
  public class PageMissingException : Exception
  {
    public PageMissingException()
    {
    }

    public PageMissingException(string message) : base(message)
    {
    }

    public PageMissingException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PageMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
