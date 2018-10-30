using System.Collections.Generic;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public interface INewsService : IDomainService
  {
    IEnumerable<NewsDTO> GetNews();
  }
}