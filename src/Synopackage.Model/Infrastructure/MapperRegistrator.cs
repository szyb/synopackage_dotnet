using ExpressMapper;
using Synopackage.Model.DTOs;
using Synopackage.Model.SPK;

namespace Synopackage
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
          .Member(dest => dest.Version, src => src.Version);
    }

  }
}
