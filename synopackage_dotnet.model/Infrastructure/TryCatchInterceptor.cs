using Castle.DynamicProxy;
using System;

namespace synopackage_dotnet
{
  //this class is to throw execption from service with full stack trace when error occurs
  //this error will be logged.
  [Obsolete("nothing is done here. Do not use")]
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
