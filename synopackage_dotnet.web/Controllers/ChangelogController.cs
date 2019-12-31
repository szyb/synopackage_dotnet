
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Services;

namespace synopackage_dotnet.Controllers
{
  ///<summary></summary>
  [Route("api/[controller]")]
  public class ChangelogController : BaseController
  {
    private IChangelogService changelogService;
    public ChangelogController(IChangelogService changelogService)
    {
      this.changelogService = changelogService;
    }

    [HttpGet("GetChangelogs")]
    public IEnumerable<ChangelogDTO> GetChangelogs()
    {
      return changelogService.GetChangelogs();
    }
  }
}
