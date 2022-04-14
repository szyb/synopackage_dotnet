using Synopackage.Model.SPK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.model.Model.DTOs
{
  public class RawSpkResultDto
  {
    public bool Success { get; private set; }
    public SpkResult SpkResult { get; private set; }
    public string ErrorMessage { get; private set; }

    public RawSpkResultDto(SpkResult spkResult, string errorMessage)
    {
      SpkResult = spkResult;
      ErrorMessage = errorMessage;
      Success = string.IsNullOrWhiteSpace(ErrorMessage);
    }
  }
}
