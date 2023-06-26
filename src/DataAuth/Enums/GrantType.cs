using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Enums
{
    public enum GrantType
    {
        ForUser = 0,

        /// <summary>
        /// This is role from consummer system.
        /// </summary>
        ForRole = 1,

        /// <summary>
        /// This is role inside DataAuth. We can use DataAuth's role system if the consummer system doesn't have roles.
        /// </summary>
        ForDataAuthRole = 2
    }
}
