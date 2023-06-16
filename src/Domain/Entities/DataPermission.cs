using Domain.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
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

        public GrantType GrantType { get; set; }

        /// <summary>
        /// Depends on GrantType, SubjectId can be either user id or role id.
        /// </summary>
        public string SubjectId { get; set; }

        public int AccessAttributeTableId { get; set; }

        public AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// When using access level Deep, this value is the root node of the granted data tree.
        /// </summary>
        public string? GrantedDataValue { get; set; }
    }
}
