using System;
using System.Runtime.Serialization;

namespace Synopackage
{
  [Serializable]
  public sealed class RepositoryException : Exception
  {
    public RepositoryException()
    {
    }

    public RepositoryException(string message) : base(message)
    {
    }

    public RepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }

  }
}