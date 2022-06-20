using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.DependencyInjection;

public static class ConfigureServices
{
  public static IServiceCollection AddSynopackageCacheOptionsManager(this IServiceCollection services)
  {
    services.AddSingleton<ICacheOptionsManager, CacheOptionsManager>();
    return services;
  }
}

