using Microsoft.AspNetCore.Mvc;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Services;
using System.Threading.Tasks;

namespace synopackage_dotnet.Controllers
{
  ///<summary></summary>
  [Route("api/[controller]")]
  public class PackagesController : BaseController
  {
    private readonly ISpkService spkService;
    private readonly ISourceService sourceService;
    private readonly IVersionService versionService;
    private readonly IModelService modelService;
    public PackagesController(
      ISpkService spkService,
      ISourceService sourceService,
      IVersionService versionService,
      IModelService modelService)
    {
      this.spkService = spkService;
      this.sourceService = sourceService;
      this.versionService = versionService;
      this.modelService = modelService;
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
      SourceServerResponseDTO response;
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
  }
}
