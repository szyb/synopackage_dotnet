using synopackage_dotnet.model.Model.DTOs;
using synopackage_dotnet.Model.Enums;
using synopackage_dotnet.Model.SPK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synopackage_dotnet.Model.Services
{
  public interface IRepositoryService
  {
    Task<SpkResult> GetRepositoryPackages(PredefinedSources predefinedSources, RepositoryRequestDto request, IList<string> userSources);
    Task<RepositoryInfoDto> GetRepositoryInfo(int majorVersion);
  }
}
