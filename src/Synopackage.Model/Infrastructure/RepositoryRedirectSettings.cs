using System.Collections.Generic;

namespace Synopackage.Model.Infrastructure
{
  public class RepositoryRedirectSettings
  {
    public bool Enabled { get; set; } = false;
    public bool OnlyRedirect { get; set; } = false;
    public List<string> Urls { get; set; }
  }
}
