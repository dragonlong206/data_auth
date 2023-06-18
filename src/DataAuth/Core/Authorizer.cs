using DataAuth.Dto;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Core
{
    internal class Authorizer
    {
        public static IEnumerable<AuthorizationResponse> GetGrantedData(string subjectId, GrantType grantType)
        {


            return Enumerable.Empty<AuthorizationResponse>();
        }
    }
}
