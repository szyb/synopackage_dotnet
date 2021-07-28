using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace synopackage_dotnet
{
  /// <summary>
  /// Main App entry point
  /// </summary>
  public static class Program
  {
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    /// <summary>
    /// Builds the web host.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The web host, ready to be run.</returns>
    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddAutofac())
            .ConfigureAppConfiguration((builderContext, config) =>
            {
              IWebHostEnvironment env = builderContext.HostingEnvironment;

              config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
            .UseStartup<Startup>()
            .Build();
  }
}
