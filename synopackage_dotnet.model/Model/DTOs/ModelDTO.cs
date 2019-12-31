using System;

namespace synopackage_dotnet.Model.DTOs
{
    public class ModelDTO :IComparable<ModelDTO>
    {
        public string Name { get; set; }
        public string Arch { get; set; }
        public string Family { get; set; }
        public string Display 
        { 
            get
            {
                return $"{Name} ({Arch})";
            }
        }

        public int CompareTo(ModelDTO other)
        {
            if  (other == null)
                return 1;
            else 
                return Name.CompareTo(other.Name);
        }
    }
}