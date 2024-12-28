using System;
using System.Runtime.Serialization;

namespace Synopackage
{
  [Serializable]
  public sealed class PageMissingException : Exception
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
  }
}
