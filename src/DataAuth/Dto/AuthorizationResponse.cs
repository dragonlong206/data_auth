using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Dto
{
    public class AuthorizationResponse
    {
        public AccessLevel AccessLevel { get; set; }
        public IEnumerable<string>? GrantedData { get; set; }
    }
}
