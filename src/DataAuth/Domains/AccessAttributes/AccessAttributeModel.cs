using DataAuth.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAuth.Domains.AccessAttributes
{
    public class AccessAttributeModel : BaseModel<int>
    {
        public AccessAttributeModel(string code)
        {
            Code = code;
        }

        [JsonConstructor]
        public AccessAttributeModel(string code, string? name, string? description)
        {
            Code = code;
            Name = name;
            Description = description;
        }

        public string Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
