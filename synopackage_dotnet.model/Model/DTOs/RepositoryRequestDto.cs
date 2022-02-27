using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synopackage_dotnet.model.Model.DTOs
{
  public class RepositoryRequestDto
  {
    public string PackageUpdateChannel {get; set;}
    public string Unique {get; set;}
    public int Build {get; set;}
    public string Language {get; set;}
    public int Major {get; set;}
    public int Micro {get; set;}
    public string Arch {get; set;}
    public int Minor {get; set;}
    public string Timezone {get; set;}
    public int Nano { get; set; }
  }
}
