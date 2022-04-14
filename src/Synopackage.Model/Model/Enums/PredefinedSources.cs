using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Enums
{
  public enum PredefinedSources
  {
    [Description("All active repositories")]
    All = 1,
    [Description("Sources choosen by Synopackage")]
    SynopackageChoice = 2,
    //[Description("Digitalbox - one link which will choose right repository for your DSM version")]
    //Digitalbox = 10,
    //[Description("Bliss - one link which will choose right repository for your DSM version")]
    //Bliss = 11,
    //[Description("Imnks - one link which will choose right repository for your DSM version")]
    //Imnks = 12,
    [Description("User defined")]
    UserDefined = 99
  }
}
