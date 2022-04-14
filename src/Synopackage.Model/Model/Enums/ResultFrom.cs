using System.ComponentModel;

namespace Synopackage.Model.Enums
{
  public enum ResultFrom
  {
    [Description("Not specified")]
    NotSpecified,
    [Description("Server")]
    Server,
    [Description("Cache")]
    Cache,
    [Description("Expired cache")]
    ExpiredCache
  }
}
