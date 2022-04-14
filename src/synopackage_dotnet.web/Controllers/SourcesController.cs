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
  public class SourcesController : BaseController
  {
    private readonly ISourceService sourceService;    
    public SourcesController(ISourceService sourceService)
    {
      this.sourceService = sourceService;
    }

    [HttpGet("GetAllSources")]
    public IActionResult GetAllSources()
    {
      return new ObjectResult(sourceService.GetAllSources());
    }

    [HttpGet("GetAllActiveSources")]
    public IEnumerable<SourceDTO> GetAllActiveSources()
    {
      return sourceService.GetAllActiveSources();
    }

    [HttpGet("GetSource")]
    public SourceDTO GetSource([FromQuery]string sourceName)
    {
      return sourceService.GetSource(sourceName);
    }
  }
}
