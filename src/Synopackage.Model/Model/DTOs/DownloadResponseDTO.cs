namespace Synopackage.Model.DTOs
{
  public class DownloadResponseDTO
  {
    public DownloadRequestDTO Request { get; private set; }
    public bool IsValid { get; private set; }
    public string Link { get; private set; }
    public string FileNme { get; set; }

    public DownloadResponseDTO(bool isValid, DownloadRequestDTO request, string link = null, string fileName = null)
    {
      this.IsValid = isValid;
      this.Request = request;
      this.Link = link;
      this.FileNme = fileName;
    }
  }
}
