using System.Collections.Generic;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
    public interface IModelService :IDomainService
    {
        IEnumerable<ModelDTO> GetAll();
        ModelDTO GetModel(string model);
    }
}