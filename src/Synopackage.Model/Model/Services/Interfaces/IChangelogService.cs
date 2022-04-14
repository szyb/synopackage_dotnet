using System.Collections.Generic;
using Synopackage.Model.DTOs;

namespace Synopackage.Model.Services
{
  public interface IChangelogService
  {
    PagingDTO<ChangelogDTO> GetChangelogs(int? page, int? itemsPerPage);
  }
}
