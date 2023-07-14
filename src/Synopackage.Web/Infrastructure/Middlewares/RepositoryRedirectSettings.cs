using System.Collections.Generic;

namespace Synopackage.Web.Infrastructure.Middlewares
{
  public class RepositoryRedirectSettings
  {
    public bool Enabled { get; set; } = false;
    public bool DisallowRepositoryQueries { get; set; } = false;
    public List<string> Urls { get; set; }
  }
}
