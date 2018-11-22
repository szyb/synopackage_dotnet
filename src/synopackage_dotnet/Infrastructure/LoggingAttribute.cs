using System;

namespace synopackage_dotnet
{
  [AttributeUsage(AttributeTargets.Method)]
  public class LoggingAttribute : Attribute
  {
    public string Property { get; set; }
    public string Value { get; set; }

    public LoggingAttribute(string property, string value)
    {
      this.Property = property;
      this.Value = value;
    }
  }
}