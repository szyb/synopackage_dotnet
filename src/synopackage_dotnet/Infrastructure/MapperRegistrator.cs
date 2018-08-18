using ExpressMapper;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet
{
    public static class MapperRegistrator
    {
        public static void Register()
        {
            Mapper.Register<SpkPackage, PackageDTO>()
                .Member(dest => dest.Name, src => src.Dname != null ? src.Dname : src.Package)
                .Member(dest => dest.Description, src => src.Desc)
                .Member(dest => dest.DownloadLink, src => src.Link)
                .Member(dest => dest.IsBeta, src => src.Beta)
                //.Member(dest => dest.ThumbnailUrl, src => src.Thumbnail != null ? src.Thumbnail[0] : null)
                .Member(dest => dest.Version, src => src.Version);
        }
        
    }
}
