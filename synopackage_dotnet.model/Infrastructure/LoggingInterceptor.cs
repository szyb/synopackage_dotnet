using System;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace synopackage_dotnet
{
  public class LoggingInterceptor : IInterceptor
  {
    public LoggingInterceptor()
    {
    }

    public void Intercept(IInvocation invocation)
    {
      var attributes = invocation.Method.GetCustomAttributes(typeof(LoggingAttribute), true);
      if (attributes.Any())
      {
        LoggingAttribute loggingAttribute = (LoggingAttribute)attributes.First();
        using (Serilog.Context.LogContext.PushProperty(loggingAttribute.Property, loggingAttribute.Value))
        {
          invocation.Proceed();
        }
      }
      else
      {
        invocation.Proceed();
      }
    }


  }
}
