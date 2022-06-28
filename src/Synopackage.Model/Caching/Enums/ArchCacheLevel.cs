using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.Enums
{
  public enum ArchCacheLevel
  {
    [Description("Caching by CPU architecture, i.e. apollolake, avoton, etc.")]
    CPU = 0,
    [Description("Caching by only by CPU architecure listed in ArchList property. All other will be cached by 'allCPUs'")]
    OnlyListed = 1,
    [Description("Caching is set to 'allCPUs' for all CPU architecures")]
    None = 2,
  }
}
