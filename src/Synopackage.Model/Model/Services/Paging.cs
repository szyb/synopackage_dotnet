namespace Synopackage.Model.Services
{
  public abstract class Paging
  {
    protected int GetToSkip(int arrayLength, int page, int itemsPerPage)
    {
      if (page > GetTotalPages(arrayLength, itemsPerPage))
        throw new PageMissingException($"Page {page} does not exists");
      int toSkip = (page - 1) * itemsPerPage;
      return toSkip;
    }


    protected int GetTotalPages(int arrayLength, int itemsPerPage)
    {
      var totalPages = 1 + (arrayLength / itemsPerPage);
      if (arrayLength % itemsPerPage == 0)
        totalPages--;
      return totalPages;
    }
  }
}
