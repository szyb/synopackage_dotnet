
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
    private readonly IChangelogService changelogService;
    public ChangelogController(IChangelogService changelogService)
    {
      this.changelogService = changelogService;
    }

    [HttpGet("GetChangelogs")]
    public ActionResult<PagingDTO<ChangelogDTO>> GetChangelogs(int? page = null, int? itemsPerPage = null)
    {
      if (page.HasValue && !itemsPerPage.HasValue)
        itemsPerPage = AppSettingsProvider.AppSettings.DefaultItemsPerPage;
      else if (!page.HasValue)
        itemsPerPage = null;
      if (page.HasValue && page <= 0)
        return BadRequest("Invalid page number");
      if (itemsPerPage.HasValue && itemsPerPage.Value <= 0)
        return BadRequest("Invalid items per page");
      try
      {
        return new ObjectResult(changelogService.GetChangelogs(page, itemsPerPage));
      }
      catch (PageMissingException pme)
      {
        return BadRequest(pme.Message);
      }
    }
  }
}
