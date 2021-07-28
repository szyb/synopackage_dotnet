
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.Services;
using System;
using System.Net;
using System.Threading.Tasks;


namespace synopackage_dotnet.Controllers
{
  [Route("api/[controller]")]
  public class DownloadController : BaseController
  {
    private readonly IDownloadSpkService downloadSpkService;
    private readonly ILogger<DownloadController> logger;

    public DownloadController(IDownloadSpkService downloadSpkService, ILogger<DownloadController> logger)
    {
      this.downloadSpkService = downloadSpkService;
      this.logger = logger;
    }

    [HttpPost("DownloadRequest")]
    public IActionResult DownloadRequest([FromBody] DownloadRequestDTO downloadRequest)
    {
      if (downloadRequest == null)
        return new BadRequestResult();

      var validation = ValidateStringParameter(nameof(downloadRequest.RequestUrl), downloadRequest.RequestUrl, 1500);
      if (!string.IsNullOrWhiteSpace(validation)) { logger.LogError(validation); return new BadRequestResult(); }
      validation = ValidateStringParameter(nameof(downloadRequest.SourceName), downloadRequest.SourceName, 100);
      if (!string.IsNullOrWhiteSpace(validation)) { logger.LogError(validation); return new BadRequestResult(); }
      validation = ValidateStringParameter(nameof(downloadRequest.PackageName), downloadRequest.PackageName, 100);
      if (!string.IsNullOrWhiteSpace(validation)) { logger.LogError(validation); return new BadRequestResult(); }

      var response = downloadSpkService.DownloadRequest(downloadRequest, Request.IsHttps);
      return new ObjectResult(response.Link);
    }


    [HttpGet("{id}")]
    public async Task ProxyDownload(Guid id)
    {
      try
      {
        if (id == Guid.Empty)
        {
          Response.StatusCode = 400;
          return;
        }
        var downloadDetails = downloadSpkService.GetDownloadDetails(id);
        if (!downloadDetails.IsValid)
        {
          if (downloadDetails.Request == null)
            Response.StatusCode = 400;
          else
            Response.StatusCode = 404;
          return;
        }

        HttpWebRequest repositoryRequest = (HttpWebRequest)WebRequest.Create(downloadDetails.Request.RequestUrl);
        HttpWebResponse repositoryResponse = (HttpWebResponse)repositoryRequest.GetResponse();

        using (var stream = repositoryResponse.GetResponseStream())
        {
          Response.StatusCode = 200;
          Response.ContentType = "application/octet-stream";
          var headers = PrepareResponseHeaders(id, repositoryResponse, downloadDetails.FileNme);
          foreach (var header in headers)
            Response.Headers.Add(header);
          byte[] buff = new byte[4096];
          while (true)
          {
            var bytesRead = await stream.ReadAsync(buff, 0, buff.Length).ConfigureAwait(false);
            if (bytesRead == 0)
              break;
            ReadOnlyMemory<byte> rom = new ReadOnlyMemory<byte>(buff, 0, bytesRead);
            await Response.BodyWriter.WriteAsync(rom).ConfigureAwait(false);
          }
        }
      }
      catch (WebException wex)
      {
        Response.StatusCode = 404;
        logger.LogError(wex, "File download error (WebException)");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "File download error");
      }
    }

    private IHeaderDictionary PrepareResponseHeaders(Guid id, HttpWebResponse newResponse, string fileName)
    {
      IHeaderDictionary headers = new HeaderDictionary();

      if (string.IsNullOrWhiteSpace(fileName))
      {
        //try get file name from Content Disposition header:
        var contentDisposition = newResponse.Headers[HeaderNames.ContentDisposition];
        if (contentDisposition != null)
        {
          fileName = downloadSpkService.GetFileNameFromContentDisposition(contentDisposition);
        }
        fileName = fileName ?? $"{id}.spk";

      }
      headers.Add(HeaderNames.ContentDisposition, $"attachment; filename=\"{fileName}\"");

      var lengthStr = newResponse.Headers[HeaderNames.ContentLength];
      if (Int64.TryParse(lengthStr, out var length) && length > 0)
      {
        headers.Add(HeaderNames.ContentLength, length.ToString());
      }
      return headers;
    }
  }
}
