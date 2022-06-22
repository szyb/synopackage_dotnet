using FluentValidation;
using Synopackage.Model.Caching.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.Validators
{
  public class CacheOptionsValidator : AbstractValidator<CacheOptions>
  {
    public CacheOptionsValidator()
    {
      RuleFor(p => p.Defaults).NotNull();
      RuleFor(p => p.SourcesOverrides).NotNull();
      RuleFor(p => p.Defaults.CacheSpkServerResponse).NotNull();
      RuleFor(p => p.Defaults).SetValidator(new CacheSettingsValidator());
      RuleFor(p => p.Defaults.CacheSpkServerResponseTimeInHours)
        .NotNull()
        .When(p => p.Defaults.CacheSpkServerResponse.HasValue && p.Defaults.CacheSpkServerResponse.Value);
      RuleFor(p => p.Defaults.CacheSpkServerResponseTimeInHoursForRepository)
        .NotNull()
        .When(p => p.Defaults.CacheSpkServerResponse.HasValue && p.Defaults.CacheSpkServerResponse.Value);
      RuleFor(p => p.Defaults.ArchCacheLevel).NotNull().IsInEnum();
      RuleFor(p => p.Defaults.ArchList)
        .NotNull()
        .When(p => p.Defaults.ArchCacheLevel == ArchCacheLevel.OnlyListed);
      RuleFor(p => p.Defaults.VersionCacheLevel).NotNull().IsInEnum();
      RuleFor(p => p.Defaults.ChannelCacheLevel).NotNull().IsInEnum();
      RuleForEach(p => p.SourcesOverrides).SetValidator(new KeyValuePairCacheSettingsValidator());
    }
  }
}
