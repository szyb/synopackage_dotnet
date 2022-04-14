using Microsoft.AspNetCore.Mvc;
using Synopackage.Model.DTOs;
using Synopackage.Model.Services;
using System.Collections.Generic;

namespace Synopackage.Controllers
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
