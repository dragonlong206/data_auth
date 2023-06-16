using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AccessAttribute : BaseEntity<int>
    {
        public AccessAttribute(string code)
        {
            Code = code;
        }

        public string Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
