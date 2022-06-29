using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.Enums
{
  public enum VersionCacheLevel
  {
    [Description("Caching by DSM version Build")]
    Build = 0,
    [Description("Caching by DSM version Major.Minor.Micro")]
    Micro = 1,
    [Description("Caching by DSM version Major.Minor")]
    Minor = 2,
    [Description("Caching by DSM version Major")]
    Major = 3
  }
}
