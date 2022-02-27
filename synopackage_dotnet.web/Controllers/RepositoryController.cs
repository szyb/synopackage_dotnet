
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using synopackage_dotnet.model.Model.DTOs;
using synopackage_dotnet.Model;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Enums;
using synopackage_dotnet.Model.Services;
using synopackage_dotnet.Model.SPK;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace synopackage_dotnet.Controllers
{
  [ApiController]
  public class RepositoryController : BaseController
  {
    private readonly int defaultMajorVersion = 7;
    private readonly IRepositoryService repositoryService;
    private readonly ILogger<RepositoryController> logger;
    private readonly IVersionService versionService;
    public RepositoryController(IRepositoryService repositoryService, ILogger<RepositoryController> logger, IVersionService versionService)
    {
      this.repositoryService = repositoryService;
      this.logger = logger;
      this.versionService = versionService;
    }

    [HttpGet("[controller]/spk/{predefined}")]
    public async Task<ActionResult<SpkResult>> GetPackages(
     [Required] string predefined,
     [FromQuery] string package_update_channel,
     [FromQuery] string unique,
     [FromQuery] int build,
     [FromQuery] int major,
     [FromQuery] int micro,
     [FromQuery] int minor,
     [FromQuery] string arch,
     [FromQuery] string language,
     [FromQuery] string timezone,
     [FromQuery] int nano
     )
    {
      Stopwatch sw = Stopwatch.StartNew();
      var request = new RepositoryRequestDto()
      {
        PackageUpdateChannel = package_update_channel,
        Unique = unique,
        Build = build,
        Language = language,
        Major = major,
        Minor = minor,
        Micro = micro,
        Arch = arch,
        Timezone = timezone,
        Nano = nano
      };
      try
      {
        if (!Enum.TryParse<PredefinedSources>(predefined, true, out var predefinedResult))
          predefinedResult = PredefinedSources.All;
        var result = await repositoryService.GetRepositoryPackages(predefinedResult, request, null).ConfigureAwait(false);
        return new ObjectResult(result);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Unable to process request");
        return Ok("This link is dedicated to your DSM only. Please add it to your Package Center (Settings => Package sources => Add)");
      }
      finally
      {
        sw.Stop();
        logger.LogInformation($"Repository returned result in {sw.ElapsedMilliseconds} ms");
      }
    }

    [HttpGet("api/[controller]/Info/{version}")]
    public async Task<RepositoryInfoDto> GetRepositoryInfo(string version)
    {
      var majorVersion = versionService.GetVersion(version)?.Major ?? defaultMajorVersion;
      return await repositoryService.GetRepositoryInfo(majorVersion);
    }

  }
}
