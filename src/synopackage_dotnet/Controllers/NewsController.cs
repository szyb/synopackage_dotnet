using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    public NewsController(INewsService newsService)
    {
      this.newsService = newsService;
    }

    [HttpGet("GetNews")]
    public IEnumerable<NewsDTO> GetNews()
    {
      return newsService.GetNews();
    }

  }
}
