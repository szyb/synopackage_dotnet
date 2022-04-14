using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Synopackage.Model;
using Synopackage.Model.Enums;
using Synopackage.Model.Services;
using System;
using System.IO;
using NLog;

namespace Synopackage
{
  /// <summary>
  /// Startup class
  /// </summary>
  public class Startup
  {
    public IConfiguration configuration { get; set; }
    private IWebHostEnvironment environment { get; set; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      this.configuration = configuration;
      this.environment = env;
    }

    private bool IsProductionOrTest()
    {
      if (environment != null)
      {
        return environment.IsProduction()
          || environment.EnvironmentName.Equals("test", StringComparison.InvariantCultureIgnoreCase);
      }
      return false;
    }


    /// <summary>
    /// Configures app the services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      if (this.IsProductionOrTest())
      {
        services.AddHttpsRedirection(options =>
        {
          options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
          options.HttpsPort = 443;
        });
      }
      services.AddLogging(c => c.AddNLog());

      NLog.LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
      services.AddMvc();

      services.AddResponseCompression(options =>
      {
        options.EnableForHttps = true;
        options.MimeTypes = new[] {
          "text/plain",
          "text/css",
          "application/javascript",
          "text/html",
          "application/xml",
          "text/xml",
          "application/json",
          "text/json"
          };
      });

      services.AddSpaStaticFiles(c =>
      {
        c.RootPath = "wwwroot";
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "synopackage_dotnet API",
          Description = "search.synopackage.com API"
        });

      });



      //appsettings
      var appSettingsSection = configuration.GetSection(nameof(AppSettings));
      services.Configure<AppSettings>(appSettingsSection);
      services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<AppSettings>>().Value);

      var appSettings = new AppSettings();
      new ConfigureFromConfigurationOptions<AppSettings>(appSettingsSection)
            .Configure(appSettings);
      services.AddSingleton(AppSettingsProvider.Create(appSettings));
      MapperRegistrator.Register();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
      var modelAssembly = typeof(SourceService).Assembly;

      builder.RegisterAssemblyTypes(modelAssembly)
          .Where(t => t.Name.EndsWith("Service") || t.Name == "BackgroundTaskQueue")
          .Except<RestSharpDownloadService>()
          .AsImplementedInterfaces()
          .EnableInterfaceInterceptors();

      builder.Register<IDownloadService>((c, p) =>
      {
        var type = p.TypedAs<DownloadServiceImplementation>();
        return type switch
        {
          DownloadServiceImplementation.RestSharp => new RestSharpDownloadService(c.Resolve<ILogger<RestSharpDownloadService>>()),
          _ => throw new NotImplementedException("Invalid download library"),
        };
      })
        .As<IDownloadService>();

      builder.RegisterType<DownloadFactory>()
        .As<IDownloadFactory>()
        .InstancePerLifetimeScope();

    }

    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="env">The hosting environment</param>
    /// <param name="hostApplicationLifetime">The host application lifetime</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IHostApplicationLifetime hostApplicationLifetime)
    {

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      if (this.IsProductionOrTest())
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
        app.UseHttpsRedirection();
      }

      app.Use(async (context, next) =>
      {
        if (!Path.HasExtension(context.Request.Path.Value)
          && !context.Request.Path.StartsWithSegments(new PathString("/api"))
          && !context.Request.Path.StartsWithSegments(new PathString("/repository/spk"))
          && !context.Request.Path.StartsWithSegments(new PathString("/notification")))
        {
          context.Request.Path = "/index.html";
          context.Response.Headers.Add("Cache-Control", "no-store,no-cache");
          context.Response.Headers.Add("Pragma", "no-cache");
          await next();
        }
        else
          await next();
      });

      app.UseDefaultFiles();
      app.UseResponseCompression();

      app.UseStaticFiles(new StaticFileOptions()
      {
        OnPrepareResponse = ctx =>
        {
          if (ctx.File.PhysicalPath.EndsWith(".woff2"))
          {
            const int durationInSeconds = 60 * 60 * 24 * 30; //30 days
            ctx.Context.Response.Headers[HeaderNames.CacheControl] = $"public,max-age={durationInSeconds}";
          }
        }
      });


      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });

      app.UseSpaStaticFiles();
      app.UseRouting();


      // CORS
      app.UseCors(config =>
      {
        if (!IsProductionOrTest())
          config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        else
          config.AllowAnyHeader().AllowAnyMethod();
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
      app.UseSpa(spa =>
      {
        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        // see https://go.microsoft.com/fwlink/?linkid=864501

        spa.Options.SourcePath = "wwwroot";
      });
      hostApplicationLifetime.ApplicationStopping.Register(OnShutdown);
    }

    private void OnShutdown()
    {
      NLog.LogManager.Shutdown();
    }
  }
}
