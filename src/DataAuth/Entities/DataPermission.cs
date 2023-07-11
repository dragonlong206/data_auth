using DataAuth.Base;
using DataAuth.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Entities
{
    public class DataPermission : BaseEntity<int>
    {
        public DataPermission(GrantType grantType, string subjectId, int accessAttributeTableId, AccessLevel accessLevel)
        {
            GrantType = grantType;
            SubjectId = subjectId;
            AccessAttributeTableId = accessAttributeTableId;
            AccessLevel = accessLevel;
        }

        public DataPermission(GrantType grantType, string subjectId, int accessAttributeTableId, AccessLevel accessLevel, string? grantedDataValue)
            : this(grantType, subjectId, accessAttributeTableId, accessLevel)
        {
            GrantedDataValue = grantedDataValue;
        }

        public GrantType GrantType { get; set; }

        /// <summary>
        /// Depends on GrantType, SubjectId can be either user id or role id.
        /// </summary>
        public string SubjectId { get; set; }

        public int AccessAttributeTableId { get; set; }

        public AccessAttributeTable? AccessAttributeTable { get; set; }

        public AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// When using access level Deep, this value is the root node of the granted data tree.
        /// </summary>
        public string? GrantedDataValue { get; set; }
    }
}
