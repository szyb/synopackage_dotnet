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
    public IEnumerable<NewsDTO> GetNews()
    {
      return newsService.GetNews();
    }

  }
}
