using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching;
public class CacheOptions
{
  /// <summary>
  /// Default settings for all sources. May be overwritten by adding dictionary entry to SourcesOverrides
  /// Most of those settings are required, unless tagged by attribute: AllowNullForDefaults
  /// </summary>
  public CacheSettings Defaults { get; set; } = new CacheSettings();

  /// <summary>
  /// Overrided cache settings for specific source. If some of certain setting is null then Defaults is taken
  /// </summary>
  public Dictionary<string, CacheSettings> SourcesOverrides { get; set; } = new Dictionary<string, CacheSettings>();
}

