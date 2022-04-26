using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Synopackage.Web.HealthChecks
{
  public class MockHealthChecksService : HealthCheckService
  {
    public override Task<HealthReport> CheckHealthAsync(Func<HealthCheckRegistration, bool> predicate, CancellationToken cancellationToken = default)
    {
      return Task.FromResult(new HealthReport(null, TimeSpan.FromSeconds(1)));
    }
  }
}
