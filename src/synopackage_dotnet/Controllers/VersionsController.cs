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
    public class VersionsController : Controller
    {
        private IVersionService versionService;
        public VersionsController(IVersionService versionService)
        {
            this.versionService = versionService;
        }

        [HttpGet("GetAll")]
        public IEnumerable<VersionDTO> GetAll()
        {
            return versionService.GetAllVersions();
        }
        [HttpGet("GetDefaultVersion")]
        public string GetDefaultVersion()
        {
            return AppSettingsProvider.AppSettings.DefaultVersion;
        }
    }
}
