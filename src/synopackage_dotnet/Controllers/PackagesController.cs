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
    public class PackagesController : Controller
    {
        private ISpkService spkService;
        private ISourceService sourceService;
        private IVersionService versionService;
        private IModelService modelService;

        public PackagesController(ISpkService spkService, ISourceService sourceService, IVersionService versionService, IModelService modelService)
        {
            this.spkService = spkService;
            this.sourceService = sourceService;
            this.versionService = versionService;
            this.modelService = modelService;
        }

        [HttpGet("GetList")]
        public IEnumerable<PackageDTO> GetList(string sourceName, string model, string version)
        {
          List<PackageDTO> list = new List<PackageDTO>();
          var versionDto = this.versionService.GetVersion(version);
          var sourceDto = this.sourceService.GetSource(sourceName);
          var modelDto = this.modelService.GetModel(model);


          if (sourceDto != null && versionDto != null && modelDto != null)
          {
            string errorMessage = null;
            var packages = this.spkService.GetPackages(sourceName,
              sourceDto.Url, 
              modelDto.Arch, 
              modelDto.Name, 
              versionDto.Major.ToString(), 
              versionDto.Minor.ToString(), 
              versionDto.Build.ToString(), 
              true, 
              sourceDto.CustomUserAgent, 
              out errorMessage );
            if (string.IsNullOrWhiteSpace(errorMessage))
              list.AddRange(packages);
          }
          return list;
        }
    }
}
