using Microsoft.AspNetCore.Mvc;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Services;
using System.Collections.Generic;

namespace synopackage_dotnet.Controllers
{
  ///<summary></summary>
  [Route("api/[controller]")]
  public class ModelsController : BaseController
  {
    private readonly IModelService modelService;
    public ModelsController(IModelService modelService)
    {
      this.modelService = modelService;
    }

    [HttpGet("GetAll")]
    public IEnumerable<ModelDTO> GetAll()
    {
      return modelService.GetAll();
    }

    [HttpGet("GetDefaultModel")]
    public IActionResult GetDefaultModel()
    {
      return new ObjectResult(AppSettingsProvider.AppSettings.DefaultModel);
    }
  }
}
