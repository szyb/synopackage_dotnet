using synopackage_dotnet.Model.DTOs;
using System.Collections.Generic;

namespace synopackage_dotnet.Model.Services
{
  public class ModelService : IModelService
  {
    public IEnumerable<ModelDTO> GetAll()
    {
      return ModelHelper.GetAllModels();
    }

    public ModelDTO GetModel(string name)
    {
      return ModelHelper.GetModelByName(name);
    }
  }
}
