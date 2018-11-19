using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace synopackage_dotnet
{
  public class TryCatchInterceptor : IInterceptor
  {
    private readonly ILogger<TryCatchInterceptor> logger;

    public TryCatchInterceptor(ILogger<TryCatchInterceptor> logger)
    {
      this.logger = logger;
    }
    public void Intercept(IInvocation invocation)
    {
      try
      {
        invocation.Proceed();
      }
      catch (Exception ex)
      {
        // logger.LogError(ex, "Unexpected error");
        throw;
      }
    }
  }
}