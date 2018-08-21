using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
    public class ModelService : IModelService
    {

        public IEnumerable<ModelDTO> GetAll()
        {
            var modelsJson = File.ReadAllText("Config/models.json");
            var models = JsonConvert.DeserializeObject<ModelDTO[]>(modelsJson);
            var list = models.ToList();
            return list;
        }
    }
}