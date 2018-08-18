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
    public class SourceController : Controller
    {
        private ISourceService sourceService;
        private ISpkService spkService;
        public SourceController(ISourceService sourceService, ISpkService spkService)
        {
            this.sourceService = sourceService;
            this.spkService = spkService;
        }

        
        [HttpGet("GetList")]
        public IEnumerable<SourceDTO> GetList()
        {
            return sourceService.GetList();
        }
        [HttpGet("GetPackages")]
        public IEnumerable<PackageDTO> GetPackagesTest()
        {
            string errorMessage = null;
            var result = spkService.GetPackages("https://packages.synocommunity.com",
                "apollolake",
                "DS718+",
                "6",
                "2", 
                "23739",
                true,
                null,
                out errorMessage);
            return result;
        }
    }
}
