using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Synopackage.Model.Caching.Exceptions;

namespace Synopackage.Model.Caching;

public partial class CacheOptionsManager : ICacheOptionsManager
{
  private CacheOptions _options;

  public CacheOptionsManager(CacheOptions cacheOptions)
  {
    _options = cacheOptions;
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

