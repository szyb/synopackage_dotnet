using System;

namespace synopackage_dotnet.Model.DTOs
{
    public class SourceDTO
    {
        public bool Active { get; set; }
        public string Name {get;set;}
        public string Url {get;set;}
        public string Www {get;set;}
        public string DisabledReason { get; set; }
        public DateTime? DisabledDate {get; set;}

    }
}