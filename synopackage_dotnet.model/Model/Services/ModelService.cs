using Microsoft.Extensions.Logging;
using synopackage_dotnet.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace synopackage_dotnet.Model.Services
{
  public class ModelService : IModelService
  {

    private readonly ILogger<ModelService> logger;

    public ModelService(ILogger<ModelService> logger)
    {
      this.logger = logger;
    }

    public IEnumerable<ModelDTO> GetAll()
    {
      return ModelHelper.GetAllModels();
    }

    public ModelDTO GetModel(string name)
    {
      return ModelHelper.GetModelByName(name);
    }

    public ModelDTO FindBestMatch(string unique, string arch)
    {
      var modelName = GetModelNameFromUnique(unique);
      if (!string.IsNullOrWhiteSpace(modelName))
      {
        var model = GetModel(modelName);
        if (model != null)
          return model;
        logger.LogWarning($"Unable to find model by name 1: {modelName} & unique {unique}");
      }
      else
        logger.LogWarning($"Unable to find model by name 2: {modelName} & unique {unique}");
      return ModelHelper.GetAllModels()
        .LastOrDefault(p => string.Equals(p.Arch, arch, StringComparison.InvariantCultureIgnoreCase));
    }

    private static string GetModelNameFromUnique(string unique)
    {
      if (string.IsNullOrWhiteSpace(unique))
        return null;
      var uniqueSplit = unique.Split('_', StringSplitOptions.RemoveEmptyEntries);
      if (uniqueSplit.Length >= 3)
        return uniqueSplit[2];
      else
        return null;
    }

  }
}
