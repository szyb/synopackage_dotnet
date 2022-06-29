using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Synopackage.Model.Caching.DependencyInjection;

public static class ConfigureServices
{
  public static IServiceCollection AddSynopackageCacheOptionsManager(this IServiceCollection services, IConfiguration configuration, string cacheConfigSectionName = "Cache")
  {
    services.AddOptions<CacheOptions>().Bind(configuration.GetSection(cacheConfigSectionName));
    services.AddSingleton<ICacheOptionsManager, CacheOptionsManager>();
    return services;
  }
}

