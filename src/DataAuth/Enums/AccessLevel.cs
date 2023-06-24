using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Enums
{
    public enum AccessLevel
    {
        /// <summary>
        /// Access data basing on current data state (local) of user.
        /// For example: Company has a lot of stores. Employee who is working in a store has permission to access data of that store.
        /// </summary>
        Local,

        /// <summary>
        /// Access data in a hierarchy, from a node to all children nodes.
        /// </summary>
        Deep,

        /// <summary>
        /// Access all data.
        /// </summary>
        Global
    }
}
