using System.Collections.Generic;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
    public interface ISourceService :IDomainService
    {
        IEnumerable<SourceDTO> GetList();
        bool ValidateSource(string source);
        SourceDTO GetSource(string source);
    }
}
