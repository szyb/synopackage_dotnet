using System.ComponentModel;

namespace synopackage_dotnet.Model.Enums
{
  public enum ResultFrom
  {
    [Description("Not specified")]
    NotSpecified,
    [Description("Server")]
    Server,
    [Description("Cache")]
    Cache
  }
}
