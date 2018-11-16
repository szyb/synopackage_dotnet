using System.ComponentModel;

namespace synopackage_dotnet.Model.Enums
{
  public enum RequestType
  {
    [Description("Not specified")]
    NotSpecified,
    [Description("Search")]
    Search,
    [Description("Browse")]
    Browse,
  }
}