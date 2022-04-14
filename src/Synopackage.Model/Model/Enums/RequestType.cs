using System.ComponentModel;

namespace Synopackage.Model.Enums
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