namespace synopackage_dotnet.Model.DTOs
{
    public class PackageDTO
    {
        public string Name {get; set;}
        public string ThumbnailUrl { get; set; }
        public string Version { get; set; }
        public string Package { get; set; }
        public string Description { get; set; }
        public bool IsBeta { get; set; }
        public string DownloadLink { get; set; }
    }
}
