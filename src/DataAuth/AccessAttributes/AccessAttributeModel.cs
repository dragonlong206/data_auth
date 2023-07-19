using DataAuth.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.AccessAttributes
{
    public class AccessAttributeModel : BaseModel<int>
    {
        public AccessAttributeModel(string code)
        {
            Code = code;
        }

        public string Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
