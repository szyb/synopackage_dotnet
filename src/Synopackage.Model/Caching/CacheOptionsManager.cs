using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching;

public class CacheOptionsManager : ICacheOptionsManager
{
  private readonly CacheOptions options;
  public CacheOptionsManager()
  {
    options = new CacheOptions();
  }

}

