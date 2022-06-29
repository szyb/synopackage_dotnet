using Synopackage.Model.Caching.Enums;
using Synopackage.Model.Caching.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Tests
{
  public class CacheSettingsValidatorTests
  {
    [Fact]
    public void ArchCacheLevelSetToOnlyListed_ListIsNull_ShouldNotBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        ArchCacheLevel = Caching.Enums.ArchCacheLevel.OnlyListed
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeFalse();
      result.Errors.Exists(p => p.PropertyName == nameof(CacheSettings.ArchList) && p.ErrorMessage.Contains("must not be empty")).ShouldBeTrue();
    }

    [Fact]
    public void CacheSpkServerResponseTimeInHours_SetToNegativeNumber_ShouldNotBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        CacheSpkServerResponseTimeInHours = -1
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeFalse();
      result.Errors.Exists(p => p.PropertyName == nameof(CacheSettings.CacheSpkServerResponseTimeInHours) && p.ErrorMessage.Contains("must be greater than or equal to '0'"))
        .ShouldBeTrue();
    }

    [Fact]
    public void CacheSpkServerResponseTimeInHoursForRepository_SetToNegativeNumber_ShouldNotBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        CacheSpkServerResponseTimeInHoursForRepository = -1
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeFalse();
      result.Errors.Exists(p => p.PropertyName == nameof(CacheSettings.CacheSpkServerResponseTimeInHoursForRepository) && p.ErrorMessage.Contains("must be greater than or equal to '0'"))
        .ShouldBeTrue();
    }

    [Fact]
    public void ArchCacheLevel_IsNotInEnum_ShouldNotBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        ArchCacheLevel = (ArchCacheLevel)100
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeFalse();
      result.Errors.Exists(p => p.PropertyName == nameof(CacheSettings.ArchCacheLevel) && p.ErrorMessage.Contains("has a range of values which does not include '100'"))
        .ShouldBeTrue();
    }

    [Fact]
    public void ArchCacheLevel_IsInEnum_ShouldBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        ArchCacheLevel = null
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void VersionCacheLevel_IsNotInEnum_ShouldNotBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        VersionCacheLevel = (VersionCacheLevel)100
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeFalse();
      result.Errors.Exists(p => p.PropertyName == nameof(CacheSettings.VersionCacheLevel) && p.ErrorMessage.Contains("has a range of values which does not include '100'"))
        .ShouldBeTrue();
    }

    [Fact]
    public void VersionCacheLevel_IsInEnum_ShouldBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        VersionCacheLevel = null
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void ChannelCacheLevel_IsNotInEnum_ShouldNotBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        ChannelCacheLevel = (ChannelCacheLevel)100
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeFalse();
      result.Errors.Exists(p => p.PropertyName == nameof(CacheSettings.ChannelCacheLevel) && p.ErrorMessage.Contains("has a range of values which does not include '100'"))
        .ShouldBeTrue();
    }

    [Fact]
    public void ChannelCacheLevel_IsInEnum_ShouldBeValid()
    {
      //arrange
      CacheSettings cacheSettings = new CacheSettings()
      {
        ChannelCacheLevel = null
      };

      //act
      CacheSettingsValidator validator = new CacheSettingsValidator();
      var result = validator.Validate(cacheSettings);

      //assert
      result.IsValid.ShouldBeTrue();
    }

  }
}
