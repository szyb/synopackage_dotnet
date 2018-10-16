using System.Collections.Generic;

namespace synopackage_dotnet.Model.DTOs
{
  public class SourceServerResponseDTO
  {
    public bool Result { get; private set; }
    public string ErrorMessage { get; private set; }
    public IEnumerable<PackageDTO> Packages { get; private set; }

    public SourceServerResponseDTO(bool result, string errorMessage, IEnumerable<PackageDTO> packages)
    {
      this.Result = result;
      this.ErrorMessage = errorMessage;
      this.Packages = packages;
    }
  }
}