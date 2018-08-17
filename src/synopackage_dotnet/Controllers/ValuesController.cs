using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace synopackage_dotnet.Controllers
{
  /// <summary>
  /// Values Controller
  /// </summary>
  /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    /// <summary>
    /// Gets some values.
    /// </summary>
    /// <returns>A collection of values</returns>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }
  }
}
