using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Synopackage.Model.Caching;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Synopackage.Web.HealthChecks
{
  public class SourceHealthCheck : IHealthCheck
  {
    private readonly string _source;
    private readonly ICacheOptionsManager _cacheOptionsManager;
    public SourceHealthCheck(string source, ICacheOptionsManager cacheOptionsManager)
    {
      _source = source;
      _cacheOptionsManager = cacheOptionsManager;
    }


    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      DateTime? lastWriteTime = null;

      try
      {
        var files = Directory.GetFiles(AppSettingsProvider.AppSettings.BackendCacheFolder, $"{_source}*.cache");
        foreach (string fileName in files)
        {
          FileSystemInfo fileInfo = new FileInfo(fileName);
          if (!lastWriteTime.HasValue)
            lastWriteTime = fileInfo.LastWriteTime;
          else if (lastWriteTime < fileInfo.LastWriteTime)
            lastWriteTime = fileInfo.LastWriteTime;
        }
        var cacheExpirationInHours = _cacheOptionsManager.GetCacheSpkServerResponseTimeInHours(_source);
        if (lastWriteTime.HasValue && lastWriteTime.Value.AddHours(cacheExpirationInHours) < DateTime.Now)
          return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, $"Last response from server was {(DateTime.Now - lastWriteTime.Value).TotalHours:00} hours ago"));
        else if (lastWriteTime.HasValue)
          return Task.FromResult(HealthCheckResult.Healthy());
        else
          return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, $"Last response from server was never recorded"));
      }
      catch (Exception ex)
      {
        return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
      }
    }
  }

}
