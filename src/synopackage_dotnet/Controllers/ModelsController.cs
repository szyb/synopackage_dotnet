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
    public class ModelsController : Controller
    {
        private IModelService modelService;
        public ModelsController(IModelService modelService)
        {
            this.modelService = modelService;
        }

        [HttpGet("GetAll")]
        public IEnumerable<ModelDTO> GetAll()
        {
            return modelService.GetAll();
        }
    }
}
