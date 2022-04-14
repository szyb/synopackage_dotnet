using Synopackage.model.Model.DTOs;
using Synopackage.Model.Enums;
using Synopackage.Model.SPK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Services
{
  public interface IRepositoryService
  {
    Task<SpkResult> GetRepositoryPackages(PredefinedSources predefinedSources, RepositoryRequestDto request, IList<string> userSources);
    Task<RepositoryInfoDto> GetRepositoryInfo(int majorVersion);
  }
}
