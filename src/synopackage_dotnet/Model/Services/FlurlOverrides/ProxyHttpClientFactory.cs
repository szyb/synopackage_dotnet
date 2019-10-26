using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace synopackage_dotnet.Model.Services
{
  public class ProxyHttpClientFactory : DefaultHttpClientFactory
  {
    private string address;

    public ProxyHttpClientFactory(string address)
    {
      this.address = address;
    }

    public override HttpMessageHandler CreateMessageHandler()
    {
      if (!string.IsNullOrWhiteSpace(address))
      {
        return new HttpClientHandler
        {
          Proxy = new WebProxy(address),
          Credentials = System.Net.CredentialCache.DefaultCredentials,
          UseProxy = true
        };
      }
      else
        return base.CreateMessageHandler();
    }
  }
}