using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Synopackage.model;
using Synopackage.model.Model.DTOs;
using Synopackage.Model.DTOs;
using Synopackage.Model.Enums;
using Synopackage.Model.SPK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Services
{
  public class RepositoryService : IRepositoryService
  {
    private readonly IVersionService versionService;
    private readonly ISpkService spkService;
    private readonly ISourceService sourceService;
    private readonly ILogger<RepositoryService> logger;

    public RepositoryService(
      IVersionService versionService,
      ISpkService spkService,
      ISourceService sourceService,
      ILogger<RepositoryService> logger)
    {
      this.versionService = versionService;
      this.spkService = spkService;
      this.sourceService = sourceService;
      this.logger = logger;
    }

    public Task<SpkResult> GetRepositoryPackages(PredefinedSources predefinedSources, RepositoryRequestDto request, IList<string> userSources)
    {
      var versionDto = this.versionService.FindBestMatch(request.Build, request.Major, request.Minor, request.Micro);
      if (versionDto == null)
        throw new RepositoryException($"Unable to determine the version {request.Major}.{request.Minor}.{request.Micro}.{request.Build}");

      return GetRepositoryPackagesInternal(predefinedSources, request.PackageUpdateChannel, request.Arch, request.Unique, versionDto, userSources);
    }

    private async Task<SpkResult> GetRepositoryPackagesInternal(
      PredefinedSources predefinedSources,
      string packageUpdateChannel,
      string arch,
      string unique,
      VersionDTO versionDto,
      IList<string> manualSources
      )
    {
      bool isBeta = string.Equals("beta", packageUpdateChannel, StringComparison.InvariantCultureIgnoreCase);
      IList<string> sources = GetSources(predefinedSources, manualSources, versionDto.Major);

      IList<Task<RawSpkResultDto>> downloadTasks = new List<Task<RawSpkResultDto>>();
      SpkResult spkResult = new SpkResult();

      foreach (var sourceName in sources)
      {
        var sourceDto = sourceService.GetSource(sourceName);
        var task = this.spkService.GetRawPackages(sourceName,
          sourceDto.Url,
          arch,
          unique,
          versionDto,
          isBeta,
          sourceDto.CustomUserAgent,
          false,
          null,
          sourceDto.UseGetMethod);
        downloadTasks.Add(task);
      }
      await Task.WhenAll(downloadTasks.ToArray()).ConfigureAwait(false);

      foreach (var task in downloadTasks)
      {
        var result = await task.ConfigureAwait(false);
        if (result.Success && result.SpkResult?.Packages != null)
          spkResult.Packages.AddRange(result.SpkResult.Packages);
      }
      return spkResult;
    }

    private IList<string> GetSources(PredefinedSources predefinedSources, IList<string> userSources, int majorVersion)
    {
      return predefinedSources switch
      {
        PredefinedSources.All => sourceService.GetActiveSources(null, majorVersion).Select(p => p.Name).ToList(),
        PredefinedSources.SynopackageChoice => sourceService.GetActiveSources(true, majorVersion).Select(p => p.Name).ToList(),
        //PredefinedSources.Digitalbox => sourceService.GetActiveSources(null, majorVersion).Where(p => p.Name.StartsWith("digitalbox")).Select(p => p.Name).ToList(),
        //PredefinedSources.Bliss => sourceService.GetActiveSources(null, majorVersion).Where(p => p.Name.StartsWith("bliss")).Select(p => p.Name).ToList(),
        //PredefinedSources.Imnks => sourceService.GetActiveSources(null, majorVersion).Where(p => p.Name.StartsWith("imnks")).Select(p => p.Name).ToList(),
        PredefinedSources.UserDefined =>
          sourceService.GetAllActiveSources()
            .Where(p => userSources.Any(x => string.Equals(x, p.Name, StringComparison.InvariantCultureIgnoreCase)))
            .Select(p => p.Name)
            .ToList(),
        _ => new List<string>(),
      };
    }

    public Task<RepositoryInfoDto> GetRepositoryInfo(int majorVersion)
    {
      var result = new RepositoryInfoDto();

      var predefinedSources = Enum.GetValues<PredefinedSources>().Where(p => p != PredefinedSources.UserDefined);
      foreach (var predefinedSource in predefinedSources)
      {
        var detail = new RepositoryInfoDetailsDto($"/repository/spk/{predefinedSource}", predefinedSource.ToString(), predefinedSource.GetEnumDescription());
        detail.Sources = GetSources(predefinedSource, null, majorVersion);
        result.Details.Add(detail);
      }
      return Task.FromResult(result);
    }
  }
}
