using System;
using System.Collections.Generic;
using System.Text;

namespace Synopackage.Generator.CacheOptionsManagerGenerator
{
  public class GeneratorPropertyDescriptor
  {
    public string PropertyName { get; set; }
    public string PropertyType { get; set; }
    public bool AllowNullForDefaults { get; set; }
  }
}
