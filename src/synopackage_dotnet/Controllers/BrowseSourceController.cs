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
    [Route("api/[controller]")]
    public class BrowseSourceController : Controller
    {
        private ISourceService sourceService;
        public BrowseSourceController(ISourceService sourceService)
        {
            this.sourceService = sourceService;
        }
        [HttpGet("GetList")]
        public IEnumerable<SourceDTO> GetList()
        {
            return sourceService.GetList();
        }
    }
}