using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Synopackage.Web.HealthChecks
{
  public class SourceHealthCheck : IHealthCheck
  {
    private readonly string _source;
    public SourceHealthCheck(string source)
    {
      _source = source;
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
        //a temporary hack for filebot to minimize number of requests to filebot server
        if (_source == "filebot" && lastWriteTime.HasValue && lastWriteTime.Value.AddHours(24) < DateTime.Now)
        {
          return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, $"Last response from server was {(DateTime.Now - lastWriteTime.Value).TotalHours:00} hours ago"));
        }
        else if (lastWriteTime.HasValue && lastWriteTime.Value.AddHours(12) < DateTime.Now)
        {
          return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, $"Last response from server was {(DateTime.Now - lastWriteTime.Value).TotalHours:00} hours ago"));
        }
        return Task.FromResult(HealthCheckResult.Healthy());
      }
      catch (Exception ex)
      {
        return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
      }
    }
  }

}
