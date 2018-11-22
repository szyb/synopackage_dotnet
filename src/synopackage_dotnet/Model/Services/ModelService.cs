using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class ModelService : IModelService
  {
    private readonly string configFile = "Config/models.json";

    private IEnumerable<ModelDTO> GetAllInternal()
    {
      var modelsJson = File.ReadAllText(configFile);
      return JsonConvert.DeserializeObject<ModelDTO[]>(modelsJson);
    }

    public IEnumerable<ModelDTO> GetAll()
    {
      return GetAllInternal();
    }

    public ModelDTO GetModel(string model)
    {
      return GetAllInternal().SingleOrDefault(item => item.Name.Equals(model, StringComparison.CurrentCultureIgnoreCase));
    }
  }
}