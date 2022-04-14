using Synopackage.Model.DTOs;
using System.Collections.Generic;

namespace Synopackage.Model.Services
{
  public interface IModelService : IDomainService
  {
    IEnumerable<ModelDTO> GetAll();
    ModelDTO GetModel(string name);
    ModelDTO FindBestMatch(string unique, string arch);
  }
}
