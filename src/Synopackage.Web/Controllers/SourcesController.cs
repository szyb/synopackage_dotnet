using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Synopackage.Model.DTOs;
using Synopackage.Model.Services;

namespace Synopackage.Controllers
{
  ///<summary></summary>
  [Route("api/[controller]")]
  public class SourcesController : BaseController
  {
    private readonly ISourceService sourceService;
    private readonly HealthCheckService healthChecksService;

    public SourcesController(ISourceService sourceService, HealthCheckService healthChecksService)
    {
      this.sourceService = sourceService;
      this.healthChecksService = healthChecksService;
    }

    [HttpGet("GetAllSources")]
    public async Task<IActionResult> GetAllSources()
    {
      var sources = sourceService.GetAllSources();
      if (AppSettingsProvider.AppSettings.HealthChecks.Enabled)
      {
        var healthReport = await this.healthChecksService.CheckHealthAsync();
        foreach (var activeSource in sources.ActiveSources)
        {
          if (healthReport.Entries.TryGetValue($"Repository: {activeSource.Name}", out var healthReportEntry))
          {
            activeSource.HealthCheckDescription = healthReportEntry.Description;
            activeSource.IsHealthy = healthReportEntry.Status == HealthStatus.Healthy;
          }
        }
      }
      return new ObjectResult(sources);
    }

    [HttpGet("GetAllActiveSources")]
    public IEnumerable<SourceDTO> GetAllActiveSources()
    {
      return sourceService.GetAllActiveSources();
    }

    [HttpGet("GetSource")]
    public SourceDTO GetSource([FromQuery] string sourceName)
    {
      return sourceService.GetSource(sourceName);
    }
  }
}
