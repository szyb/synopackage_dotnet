using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using System.Web;

namespace Synopackage.Web.Infrastructure.Middlewares
{
  public class RepositoryRedirectMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly Random rnd;
    public RepositoryRedirectMiddleware(RequestDelegate next)
    {
      _next = next;
      rnd = new Random();
    }
    public async Task InvokeAsync(HttpContext httpContext, IOptions<RepositoryRedirectSettings> options)
    {
      if (!httpContext.Request.Path.StartsWithSegments(new PathString("/repository/spk")) || options == null || !options.Value.Enabled)
      {
        await _next(httpContext).ConfigureAwait(false);
        return;
      }
      var urls = options.Value.Urls;
      if (!options.Value.DisallowRepositoryQueries)
        urls.Add("self");

      var index = rnd.Next(urls.Count);
      if (urls[index] == "self")
      {
        await _next(httpContext).ConfigureAwait(false);
        return;
      }
      Uri? uri;

      Uri.TryCreate(new Uri(urls[index]), $"{httpContext.Request.Path}{httpContext.Request.QueryString}", out uri);

      httpContext.Response.Redirect(uri.ToString(), false);

    }
  }
}
