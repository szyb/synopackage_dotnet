using Newtonsoft.Json.Converters;

namespace Synopackage.Generator.Entities
{
  public class DateFormatConverter : IsoDateTimeConverter
  {
    public DateFormatConverter(string format)
    {
      DateTimeFormat = format;
    }
  }
}
