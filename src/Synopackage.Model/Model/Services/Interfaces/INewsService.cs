using System.Collections.Generic;
using Synopackage.Model.DTOs;

namespace Synopackage.Model.Services
{
  public interface INewsService : IDomainService
  {
    PagingDTO<NewsDTO> GetNews(int? page, int? itemsPerPage);
  }
}
