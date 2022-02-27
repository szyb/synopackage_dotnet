using synopackage_dotnet.Model.DTOs;
using System.Collections.Generic;

namespace synopackage_dotnet.Model.Services
{
  public interface IModelService : IDomainService
  {
    IEnumerable<ModelDTO> GetAll();
    ModelDTO GetModel(string name);
    ModelDTO FindBestMatch(string unique, string arch);
  }
}
