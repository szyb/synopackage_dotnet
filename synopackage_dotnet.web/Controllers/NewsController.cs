using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Services;

namespace synopackage_dotnet.Controllers
{
  ///<summary></summary>
  [Route("api/[controller]")]
  public class NewsController : BaseController
  {
    private INewsService newsService;
    private readonly ILogger<NewsController> logger;
    public NewsController(INewsService newsService, ILogger<NewsController> logger)
    {
      this.logger = logger;
      this.newsService = newsService;
    }

    [HttpGet("GetNews")]
    public ActionResult<PagingDTO<NewsDTO>> GetNews(int? page = null, int? itemsPerPage = null)
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
        return new ObjectResult(newsService.GetNews(page, itemsPerPage));
      }
      catch (PageMissingException pme)
      {
        return BadRequest(pme.Message);
      }
    }

  }
}
