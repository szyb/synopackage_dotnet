using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Synopackage.Model.Caching.Exceptions;
using Synopackage.Model.Caching.Validators;
using Synopackage.Model.Caching.Enums;
using Synopackage.Model.DTOs;

namespace Synopackage.Model.Caching;

public partial class CacheOptionsManager : ICacheOptionsManager
{
  private CacheOptions _options;

  public CacheOptionsManager(IOptions<CacheOptions> cacheOptions)
  {
    _options = cacheOptions.Value;
    CacheOptionsValidator validator = new CacheOptionsValidator();
    validator.ValidateAndThrow(_options);
  }

  public const string AllCPUsConstant = "allCPUs";
  public string GetArchStringForCacheFile(string arch, string sourceName = null)
  {
    var archLevel = GetArchCacheLevel(sourceName);
    switch (archLevel)
    {
      case ArchCacheLevel.CPU: return arch;
      case ArchCacheLevel.None: return AllCPUsConstant;
      case ArchCacheLevel.OnlyListed:
        if (GetArchList(sourceName).Contains(arch))
          return arch;
        else
          return AllCPUsConstant;
      default:
        return arch;
    }
  }

  public string GetVersionStringForCacheFile(VersionDTO version, string sourceName = null)
  {
    var versionCacheLevel = GetVersionCacheLevel(sourceName);
    switch (versionCacheLevel)
    {
      case VersionCacheLevel.Build: return version.Build.ToString();
      case VersionCacheLevel.Major: return $"DSM{version.Major}";
      case VersionCacheLevel.Minor: return $"DSM{version.Major}-{version.Minor}";
      case VersionCacheLevel.Micro: return $"DSM{version.Major}-{version.Minor}-{version.Micro}";
      default:
        return version.Build.ToString();
    }
  }

  public const string BetaChannelString = "beta";
  public const string StableChannelString = "stable";

  public string GetChannelStringForCacheFile(bool isBeta, string sourceName = null)
  {
    var channelCacheLevel = GetChannelCacheLevel(sourceName);
    switch (channelCacheLevel)
    {
      case ChannelCacheLevel.Requested: return isBeta ? BetaChannelString : StableChannelString;
      case ChannelCacheLevel.Fixed: return StableChannelString;
      default:
        return StableChannelString;
    }
  }

  #region helper methods
  private CacheSettings GetCacheSettings(string sourceName = null)
  {
    if (sourceName == null)
      return _options.Defaults;
    if (_options.SourcesOverrides.ContainsKey(sourceName))
      return CombineCacheSettings(sourceName, _options.SourcesOverrides[sourceName], _options.Defaults);
    else
      return _options.Defaults;
  }
  #endregion
}

