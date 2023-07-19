using DataAuth.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Entities
{
    public class AccessAttribute : BaseEntity<int>
    {
        public AccessAttribute(string code)
        {
            Code = code;
        }

        public AccessAttribute(string code, string? name, string? description)
            : this(code)
        {
            Name = name;
            Description = description;
        }

        public string Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
