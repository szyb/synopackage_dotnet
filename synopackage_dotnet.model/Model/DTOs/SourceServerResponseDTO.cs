using System.Collections.Generic;
using synopackage_dotnet.Model.Enums;

namespace synopackage_dotnet.Model.DTOs
{
  public class SourceServerResponseDTO
  {
    public bool Result { get; private set; }
    public string ErrorMessage { get; private set; }
    public ResultFrom ResultFrom { get; private set; }
    public double? CacheOld { get; private set; }
    public ParametersDTO Parameters { get; private set; }
    public IEnumerable<PackageDTO> Packages { get; private set; }

    public SourceServerResponseDTO(bool result, string errorMessage, ParametersDTO parameters, IEnumerable<PackageDTO> packages, ResultFrom resultFrom, double? cacheOld)
    {
      this.Result = result;
      this.ErrorMessage = errorMessage;
      this.Parameters = parameters;
      this.Packages = packages;
      this.ResultFrom = resultFrom;
      this.CacheOld = cacheOld;
    }
  }
}
