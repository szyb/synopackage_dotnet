using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching;
public class CacheOptions
{
  public CacheSettings Defaults { get; set; } = new CacheSettings();
  public Dictionary<string, CacheSettings> SourcesOverrides { get; set; } = new Dictionary<string, CacheSettings>();
}

