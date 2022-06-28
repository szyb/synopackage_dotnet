using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.Enums
{
  public enum ChannelCacheLevel
  {
    [Description("Caching as user requested: stable or beta")]
    Requested = 0,
    [Description("Caching is fixed to stable. User requested channel is ignored.")]
    Fixed = 1
  }
}
