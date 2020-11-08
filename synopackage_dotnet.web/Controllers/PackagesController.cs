using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Services;

namespace synopackage_dotnet.Controllers
{
  ///<summary></summary>
  [Route("api/[controller]")]
  public class PackagesController : BaseController
  {
    private ISpkService spkService;
    private ISourceService sourceService;
    private IVersionService versionService;
    private IModelService modelService;
    private readonly ILogger<PackagesController> logger;

    public PackagesController(ISpkService spkService, ISourceService sourceService, IVersionService versionService, IModelService modelService, ILogger<PackagesController> logger)
    {
      this.spkService = spkService;
      this.sourceService = sourceService;
      this.versionService = versionService;
      this.modelService = modelService;
      this.logger = logger;
    }

    [HttpGet("GetSourceServerResponse")]
    public async Task<ActionResult<SourceServerResponseDTO>> GetSourceServerResponse([FromQuery] string sourceName, [FromQuery] string model, [FromQuery] string version, [FromQuery] bool isBeta, [FromQuery] string keyword, [FromQuery] bool isSearch)
    {
      var validation = ValidateStringParameter(nameof(sourceName), sourceName, 100);
      if (!string.IsNullOrWhiteSpace(validation)) return BadRequest(validation);
      validation = ValidateStringParameter(nameof(model), model, 100);
      if (!string.IsNullOrWhiteSpace(validation)) return BadRequest(validation);
      validation = ValidateStringParameter(nameof(version), version, 100);
      if (!string.IsNullOrWhiteSpace(validation)) return BadRequest(validation);
      validation = ValidateStringParameter(nameof(keyword), keyword, 300);
      if (!string.IsNullOrWhiteSpace(validation)) return BadRequest(validation);
      SourceServerResponseDTO response = null;
      if (model == null)
        model = AppSettingsProvider.AppSettings.DefaultModel;
      if (version == null)
        version = AppSettingsProvider.AppSettings.DefaultVersion;

      var versionDto = this.versionService.GetVersion(version);
      var sourceDto = this.sourceService.GetSource(sourceName);
      var modelDto = this.modelService.GetModel(model);

      if (sourceDto != null && versionDto != null && modelDto != null)
      {
        response = await this.spkService.GetPackages(sourceName,
          sourceDto.Url,
          modelDto.Arch,
          modelDto.Name,
          versionDto,
          isBeta,
          sourceDto.CustomUserAgent,
          isSearch,
          keyword,
          sourceDto.UseGetMethod);
      }
      else
      {
        ParametersDTO parameters = new ParametersDTO(sourceName, model, version, isBeta, keyword);
        response = new SourceServerResponseDTO(false, "Given parameters are not valid", parameters, null, Model.Enums.ResultFrom.NotSpecified, null);
      }
      return new ObjectResult(response);
    }

    [HttpPost("DownloadRequest")]
    public IActionResult DownloadRequest([FromBody] DownloadRequestDTO downloadRequest)
    {
      if (downloadRequest == null)
        return new BadRequestResult();

      var validation = ValidateStringParameter(nameof(downloadRequest.RequestUrl), downloadRequest.RequestUrl, 1500);
      if (!string.IsNullOrWhiteSpace(validation)) { logger.LogError(validation); return new BadRequestResult(); }
      validation = ValidateStringParameter(nameof(downloadRequest.SourceName), downloadRequest.SourceName, 100);
      if (!string.IsNullOrWhiteSpace(validation)) { logger.LogError(validation); return new BadRequestResult(); }
      validation = ValidateStringParameter(nameof(downloadRequest.PackageName), downloadRequest.PackageName, 100);
      if (!string.IsNullOrWhiteSpace(validation)) { logger.LogError(validation); return new BadRequestResult(); }

      sourceService.DownloadRequest(downloadRequest);
      return new NoContentResult();
    }
  }


}
