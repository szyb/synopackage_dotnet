using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.Validators
{
  public class CacheSettingsValidator : AbstractValidator<CacheSettings>
  {
    public CacheSettingsValidator()
    {
      RuleFor(p => p.CacheSpkServerResponseTimeInHours).GreaterThanOrEqualTo(0);
      RuleFor(p => p.CacheSpkServerResponseTimeInHoursForRepository).GreaterThanOrEqualTo(0);
      RuleFor(p => p.ArchCacheLevel).IsInEnum().When(p => p.ArchCacheLevel != null);
      RuleFor(p => p.VersionCacheLevel).IsInEnum().When(p => p.VersionCacheLevel != null);
      RuleFor(p => p.ChannelCacheLevel).IsInEnum().When(p => p.ChannelCacheLevel != null);
      RuleFor(p => p.ArchList).NotNull().When(p => p.ArchCacheLevel == Enums.ArchCacheLevel.OnlyListed);
    }
  }
}
