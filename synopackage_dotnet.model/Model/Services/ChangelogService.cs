using synopackage_dotnet.Model.DTOs;
using System.Linq;

namespace synopackage_dotnet.Model.Services
{
  public class ChangelogService : Paging, IChangelogService
  {
    private readonly string configFile = "Config/changelog.json";

    private PagingDTO<ChangelogDTO> GetChangelogsInternal(int? page, int? itemsPerPage)
    {
      var changelogs = ChangelogHelper.GetChangelogs();

      if (page.HasValue && itemsPerPage.HasValue)
      {
        var toSkip = GetToSkip(changelogs.Length, page.Value, itemsPerPage.Value);

        PagingDTO<ChangelogDTO> result = new PagingDTO<ChangelogDTO>(
          GetTotalPages(changelogs.Length, itemsPerPage.Value),
          page.Value,
          itemsPerPage.Value,
          changelogs.Skip(toSkip).Take(itemsPerPage.Value).ToArray());
        return result;
      }
      else
      {
        return new PagingDTO<ChangelogDTO>(1, 1, changelogs.Length, changelogs);
      }
    }

    public PagingDTO<ChangelogDTO> GetChangelogs(int? page, int? itemsPerPage)
    {
      return GetChangelogsInternal(page, itemsPerPage);
    }
  }
}
