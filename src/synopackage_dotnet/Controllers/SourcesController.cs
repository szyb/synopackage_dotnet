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
    private ISourceService sourceService;
    private ISpkService spkService;
    public SourcesController(ISourceService sourceService, ISpkService spkService)
    {
      this.sourceService = sourceService;
      this.spkService = spkService;
    }


    [HttpGet("GetAllSources")]
    public IActionResult GetAllSources()
    {
      return new ObjectResult(sourceService.GetAllSources());
    }

    [HttpGet("GetAllActiveSources")]
    public IEnumerable<SourceLiteDTO> GetAllActiveSources()
    {
      return sourceService.GetAllActiveSources();
    }
  }
}
