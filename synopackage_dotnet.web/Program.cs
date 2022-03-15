using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using System;

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
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
      BuildWebHost(args).Run();
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      var logger = LogManager.GetCurrentClassLogger();
      if (e.IsTerminating)
      {
        logger.Fatal("App is crashed!", e.ExceptionObject);
      }
      else
        logger.Error("Unhandled exception", e.ExceptionObject);
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
