using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching.Validators
{
  public class KeyValuePairCacheSettingsValidator : AbstractValidator<KeyValuePair<string, CacheSettings>>
  {
    public KeyValuePairCacheSettingsValidator()
    {
      RuleFor(p => p.Value).SetValidator(new CacheSettingsValidator());
    }
  }
}
