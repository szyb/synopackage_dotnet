using System;

namespace synopackage_dotnet.Model.DTOs
{
  public class ModelDTO : IComparable<ModelDTO>
  {
    public string Name { get; set; }
    public string Arch { get; set; }
    public string Family { get; set; }
    public string Display => $"{Name} ({Arch})";
    public int CompareTo(ModelDTO other) => other == null ? 1 : Name.CompareTo(other.Name);
  }
}
