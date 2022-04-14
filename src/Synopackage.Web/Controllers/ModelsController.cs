using Microsoft.AspNetCore.Mvc;
using Synopackage.Model.DTOs;
using Synopackage.Model.Services;
using System.Collections.Generic;

namespace Synopackage.Controllers
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
