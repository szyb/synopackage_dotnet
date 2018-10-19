namespace synopackage_dotnet.Model.DTOs
{
  public class ParametersDTO
  {
    public string SourceName { get; set; }
    public string Model { get; set; }
    public string Version { get; set; }
    public bool IsBeta { get; set; }

    public ParametersDTO(string sourceName, string model, VersionDTO versionDto, bool isBeta)
    {
      this.SourceName = sourceName;
      this.Model = model;
      this.Version = versionDto.ToString();
      this.IsBeta = isBeta;
    }

    public ParametersDTO(string sourceName, string model, string version, bool isBeta)
    {
      this.SourceName = sourceName;
      this.Model = model;
      this.Version = version;
      this.IsBeta = isBeta;
    }
  }
}