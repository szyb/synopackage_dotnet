using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.Exceptions
{
  [Serializable]
  public class CacheConfigSectionNotFoundException : Exception
  {
    public CacheConfigSectionNotFoundException()
    {
    }

    public CacheConfigSectionNotFoundException(string message) : base($"Config section '{message}' not found")
    {
    }

    public CacheConfigSectionNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected CacheConfigSectionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
