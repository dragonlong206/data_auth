﻿using DataAuth.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Entities
{
    public class Role : BaseEntity<int>
    {
        public Role(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }
    }
}
