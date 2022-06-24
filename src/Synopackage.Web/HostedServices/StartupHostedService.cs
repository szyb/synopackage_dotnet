using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Synopackage.Model.Caching;
using Synopackage.Model.DTOs;
using Synopackage.Model.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Synopackage.Web.HostedServices
{
  public class StartupHostedService : IHostedService
  {
    private readonly ILogger<StartupHostedService> _logger;
    private readonly ICacheOptionsManager cacheOptionsManager;
    private readonly IVersionService versionService;
    public StartupHostedService(ILogger<StartupHostedService> logger, ICacheOptionsManager cacheOptionsManager, IVersionService versionService)
    {
      _logger = logger;
      this.cacheOptionsManager = cacheOptionsManager;
      this.versionService = versionService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      await CreateCacheFolders(cancellationToken);
      await MigrateCacheFilesOneTimeJob(cancellationToken);
      await CleanupCacheFolders(cancellationToken);
      stopwatch.Stop();
      _logger.LogInformation($"Startup hosted service finished in {stopwatch.ElapsedMilliseconds}ms");

    }

    private Task CleanupCacheFolders(CancellationToken cancellationToken)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      int filesDeleted = 0;
      try
      {
        var sources = new List<SourceDTO>(Model.SourceHelper.ActiveSources);
        foreach (var source in sources)
        {
          _logger.LogInformation($"Cleanup processing {source.Name} source...");
          if (cancellationToken.IsCancellationRequested)
          {
            _logger.LogInformation("Cancellation is requested. Exiting...");
            return Task.CompletedTask;
          }

          var cleanupFolder = Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, source.Name);
          var allFiles = new DirectoryInfo(cleanupFolder).GetFiles("*.cache")
            .Where(p => p.LastWriteTime.Date.AddDays(60) < DateTime.Today)
            .Select(p => p.Name);

          foreach (var sourceFile in allFiles)
          {
            try
            {
              FileInfo fi = new FileInfo(Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, source.Name, sourceFile));
              if (fi.Exists)
              {
                File.Delete(fi.FullName);
                filesDeleted++;
              }
            }
            catch (Exception ex)
            {
              _logger.LogError(ex, $"Unable to delete file {sourceFile}");
            }
          }

          _logger.LogInformation($"Finished cleanup processing {source.Name} source. Time elapsed {stopwatch.ElapsedMilliseconds} ms");
        }

        return Task.CompletedTask;
      }
      finally
      {
        stopwatch.Stop();
        _logger.LogInformation($"Cleanup files completed in {stopwatch.ElapsedMilliseconds}ms. Files deleted: {filesDeleted}.");
      }
    }

    #region One time migration - to be deleted after deployment
    private Task MigrateCacheFilesOneTimeJob(CancellationToken cancellationToken)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        if (!IsFirstRun())
          return Task.CompletedTask;
        var sources = new List<SourceDTO>(Model.SourceHelper.ActiveSources);
        sources.AddRange(Model.SourceHelper.InactiveSources);

        //first synnocommunity
        var synocommunitySourceDto = sources.Find(p => p.Name == "synocommunity");
        if (synocommunitySourceDto != null)
        {
          sources.Remove(synocommunitySourceDto);
          sources.Insert(0, synocommunitySourceDto);
        }

        var directory = new DirectoryInfo(AppSettingsProvider.AppSettings.BackendCacheFolder);
        var allFiles = directory.GetFiles("*.cache", SearchOption.TopDirectoryOnly)
          .OrderBy(p => p.Name)
          .ThenByDescending(p => p.LastWriteTime)
          .Select(p => p.Name);
        foreach (var source in sources)
        {
          _logger.LogInformation($"Processing {source.Name} source...");
          if (cancellationToken.IsCancellationRequested)
          {
            _logger.LogInformation("Cancellation is requested. Exiting...");
            return Task.CompletedTask;
          }

          var targetFolder = Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, source.Name);
          var sourceFiles = allFiles.Where(p => p.StartsWith($"{source.Name}_"));
          foreach (var sourceFile in sourceFiles)
          {
            FileInfo fi = new FileInfo(Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, sourceFile));
            if (fi.Exists)
            {
              if (source.Active && fi.LastWriteTime.AddDays(60) < DateTime.Today)
                MigrateCacheFile(source, fi.FullName, targetFolder);
              else if (!source.Active)
                MigrateCacheFile(source, fi.FullName, targetFolder);
            }
          }


          _logger.LogInformation($"Finished processing {source.Name} source. Time elapsed {stopwatch.ElapsedMilliseconds} ms");
        }

        return Task.CompletedTask;
      }
      finally
      {
        stopwatch.Stop();
        _logger.LogInformation($"Moving files completed in {stopwatch.ElapsedMilliseconds}ms");
      }
    }

    private void MigrateCacheFile(SourceDTO source, string sourceFile, string targetFolder)
    {
      try
      {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile);
        var split = fileNameWithoutExtension.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
        var versionPart = split[2];
        if (!Int32.TryParse(versionPart, out var buildNumber))
        {
          _logger.LogWarning($"Could not identify DSM version build {versionPart} for {sourceFile}");
          return;
        }
        var channelPart = split.LastOrDefault();
        var isBeta = false;
        if (channelPart == "beta")
          isBeta = true;

        VersionDTO version = Model.VersionHelper.FindByBuild(buildNumber);

        StringBuilder sb = new StringBuilder();
        sb.Append(source.Name);
        sb.Append('_');
        sb.Append(cacheOptionsManager.GetArchStringForCacheFile(split[1], source.Name));
        sb.Append('_');
        sb.Append(cacheOptionsManager.GetVersionStringForCacheFile(version, source.Name));
        sb.Append('_');
        sb.Append(cacheOptionsManager.GetChannelStringForCacheFile(isBeta, source.Name));
        sb.Append(".cache");

        FileInfo fileInfo = new FileInfo(Path.Combine(targetFolder, sb.ToString()));
        if (!fileInfo.Exists)
          File.Move(sourceFile, fileInfo.FullName);
        else
          File.Delete(sourceFile);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Unable to migrate file {sourceFile}");
      }
    }

    private bool IsFirstRun()
    {
      var folder = Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, "synocommunity");
      if (!Directory.Exists(folder)) //something is wrong
        return false;
      var files = Directory.GetFiles(folder);
      return !files.Any();
    }
    #endregion

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation($"Startup hosted service has stopped");
      return Task.CompletedTask;
    }

    private Task CreateCacheFolders(CancellationToken cancellationToken)
    {
      var sources = new List<SourceDTO>(Model.SourceHelper.ActiveSources);
      sources.AddRange(Model.SourceHelper.InactiveSources);
      foreach (var source in sources)
      {
        if (cancellationToken.IsCancellationRequested)
        {
          _logger.LogInformation($"Startup hosted service has been cancelled");
          break;
        }
        var folder = Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, source.Name);
        Directory.CreateDirectory(folder);
      }
      return Task.CompletedTask;
    }

  }
}
