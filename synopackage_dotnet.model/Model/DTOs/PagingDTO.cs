using System.Collections.Generic;

namespace synopackage_dotnet.Model.DTOs
{
  public class PagingDTO<T>
  {
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
    public IEnumerable<T> Items { get; set; }
    public PagingDTO(int totalPages, int currentPage, int itemsPerPage)
    {
      this.TotalPages = totalPages;
      this.CurrentPage = currentPage;
      this.ItemsPerPage = itemsPerPage;
    }
    public PagingDTO(int totalPages, int currentPage, int itemsPerPage, IEnumerable<T> items) : this(totalPages, currentPage, itemsPerPage)
    {
      this.Items = items;
    }

  }
}
