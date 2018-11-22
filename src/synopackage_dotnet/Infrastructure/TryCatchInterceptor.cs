using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace synopackage_dotnet
{
  //this class is to throw execption from service with full stack trace when error occurs
  //this error will be logged.
  public class TryCatchInterceptor : IInterceptor
  {
    public TryCatchInterceptor()
    {      
    }

    public void Intercept(IInvocation invocation)
    {
      try
      {
        invocation.Proceed();
      }
      catch 
      {
        throw;
      }
    }
  }
}
