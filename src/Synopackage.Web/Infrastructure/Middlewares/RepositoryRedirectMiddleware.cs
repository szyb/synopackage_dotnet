using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using System.Web;
using Synopackage.Model.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;

namespace Synopackage.Web.Infrastructure.Middlewares
{
  public class RepositoryRedirectMiddleware :IMiddleware
  {
    private readonly Random rnd;
    private readonly List<string> _redirects;
    private readonly bool isEnabled = false;
    public RepositoryRedirectMiddleware(IOptions<RepositoryRedirectSettings> options)
    {
      rnd = new Random();
      isEnabled = options.Value.Enabled;

      _redirects = new List<string>(options.Value.Urls);
      if (!options.Value.OnlyRedirect)
        _redirects.Add("self");
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      if (!context.Request.Path.StartsWithSegments(new PathString("/repository/spk")) || !isEnabled)
      {
        await next(context).ConfigureAwait(false);
        return;
      }
      var index = rnd.Next(_redirects.Count);
      if (_redirects[index] == "self")
      {
        await next(context).ConfigureAwait(false);
        return;
      }

      if (Uri.TryCreate(new Uri(_redirects[index]), $"{context.Request.Path}{context.Request.QueryString}", out var uri))
        context.Response.Redirect(uri.ToString(), false, true);
      else
        await next(context).ConfigureAwait(false); //just in case
    }
  }
}
