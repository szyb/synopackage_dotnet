using System.Collections.Generic;

namespace synopackage_dotnet.Model.DTOs
{
  public class SourceServerResponseDTO
  {
    public bool Result { get; private set; }
    public string ErrorMessage { get; private set; }

    public ParametersDTO Parameters { get; private set; }
    public IEnumerable<PackageDTO> Packages { get; private set; }

    public SourceServerResponseDTO(bool result, string errorMessage, ParametersDTO parameters, IEnumerable<PackageDTO> packages)
    {
      this.Result = result;
      this.ErrorMessage = errorMessage;
      this.Parameters = parameters;
      this.Packages = packages;
    }
  }
}