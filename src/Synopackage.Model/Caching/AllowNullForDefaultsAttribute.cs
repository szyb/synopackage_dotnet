using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class AllowNullForDefaultsAttribute : Attribute
  {
  }
}
