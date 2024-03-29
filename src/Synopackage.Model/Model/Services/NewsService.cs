﻿using Newtonsoft.Json;
using Synopackage.Model.DTOs;
using System.IO;
using System.Linq;

namespace Synopackage.Model.Services
{
  public class NewsService : Paging, INewsService
  {
    private PagingDTO<NewsDTO> GetNewsInternal(int? page, int? itemsPerPage)
    {
      var news = NewsHelper.GetNews();

      if (page.HasValue && itemsPerPage.HasValue)
      {
        var toSkip = GetToSkip(news.Length, page.Value, itemsPerPage.Value);

        PagingDTO<NewsDTO> result = new PagingDTO<NewsDTO>(
          GetTotalPages(news.Length, itemsPerPage.Value),
          page.Value,
          itemsPerPage.Value,
          news.Skip(toSkip).Take(itemsPerPage.Value).ToArray());
        return result;
      }
      else
      {
        return new PagingDTO<NewsDTO>(1, 1, news.Length, news);
      }
    }

    public PagingDTO<NewsDTO> GetNews(int? page, int? itemsPerPage)
    {
      return GetNewsInternal(page, itemsPerPage);
    }
  }
}
