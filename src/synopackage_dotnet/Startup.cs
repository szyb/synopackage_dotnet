using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using Autofac;
using Autofac.Builder;
using synopackage_dotnet.Model.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Serilog;

namespace synopackage_dotnet
{
  /// <summary>
  /// Startup class
  /// </summary>
  public class Startup
  {
    public IConfiguration configuration { get; set; }

    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      this.configuration = configuration;
    }


    /// <summary>
    /// Configures app the services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddSpaStaticFiles(c =>
      {
        c.RootPath = "wwwroot";
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info
        {
          Version = "v1",
          Title = "synopackage_dotnet API",
          Description = "A simple example ASP.NET Core Web API"
        });
        // Set the comments path for the Swagger JSON and UI.
        // var basePath = AppContext.BaseDirectory;
        //       var xmlPath = Path.Combine(basePath, "synopackage_dotnet.xml");
        //       c.IncludeXmlComments(xmlPath);
      });

      //appsettings
      var appSettingsSection = configuration.GetSection(nameof(AppSettings));
      services.Configure<AppSettings>(appSettingsSection);
      services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<AppSettings>>().Value);

      var appSettings = new AppSettings();
      new ConfigureFromConfigurationOptions<AppSettings>(appSettingsSection)
            .Configure(appSettings);
      services.AddSingleton(new AppSettingsProvider(appSettings));

      MapperRegistrator.Register();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
      var modelAssembly = typeof(SourceService).Assembly;

      builder.RegisterAssemblyTypes(modelAssembly)
          .Where(t => t.Name.EndsWith("Service") || t.Name == "BackgroundTaskQueue")
          .AsImplementedInterfaces();

    }

    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="env">The hosting environment</param>
    /// <param name="loggerFactory">The logger factory</param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
      loggerFactory.AddSerilog(logger);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseDefaultFiles();
      app.UseStaticFiles();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });
      app.UseStaticFiles();
      app.UseSpaStaticFiles();
      app.UseMvc(routes =>
      {
        routes.MapRoute(name: "default", template: "{controller}/{action=index}/{id}");
      });

      // CORS
      app.UseCors(config =>
        config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

      app.UseSpa(spa =>
      {
        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        // see https://go.microsoft.com/fwlink/?linkid=864501

        spa.Options.SourcePath = "wwwroot";

        if (env.IsDevelopment())
        {
          spa.UseAngularCliServer(npmScript: "start");
        }
      });
    }
  }
}
