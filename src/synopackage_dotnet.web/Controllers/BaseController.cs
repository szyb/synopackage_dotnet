using System;
using Microsoft.AspNetCore.Mvc;

namespace synopackage_dotnet.Controllers
{
  public abstract class BaseController : Controller
  {
    protected string ValidateStringParameter(string parameterName, string parameterValue, int maxLength)
    {
      if (parameterValue != null && parameterValue.Length > maxLength)
        return $"Parameter {parameterName} is too long. Max length is {maxLength}.";
      return null;
    }
  }
}