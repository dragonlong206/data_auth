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
        public string Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
