using Microsoft.AspNetCore.Mvc;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Services;
using System.Collections.Generic;

namespace synopackage_dotnet.Controllers
{
  ///<summary></summary>
  [Route("api/[controller]")]
  public class VersionsController : BaseController
  {
    private readonly IVersionService versionService;
    public VersionsController(IVersionService versionService)
    {
      this.versionService = versionService;
    }

    [HttpGet("GetAll")]
    public IEnumerable<VersionDTO> GetAll()
    {
      return versionService.GetAllVersions();
    }

    [HttpGet("GetDefaultVersion")]
    public IActionResult GetDefaultVersion()
    {
      return new ObjectResult(AppSettingsProvider.AppSettings.DefaultVersion);
    }
  }
}
