using DataAuth.Base;
using DataAuth.Enums;

namespace DataAuth.Entities
{
    public class DataPermission : BaseEntity<int>
    {
        public DataPermission(
            GrantType grantType,
            string subjectId,
            int accessAttributeTableId,
            AccessLevel accessLevel
        )
        {
            GrantType = grantType;
            SubjectId = subjectId;
            AccessAttributeTableId = accessAttributeTableId;
            AccessLevel = accessLevel;
            FunctionCode = Enums.FunctionCode.All;
        }

        public DataPermission(
            GrantType grantType,
            string subjectId,
            int accessAttributeTableId,
            AccessLevel accessLevel,
            string? grantedDataValue
        )
            : this(grantType, subjectId, accessAttributeTableId, accessLevel)
        {
            GrantedDataValue = grantedDataValue;
        }

        public DataPermission(
            GrantType grantType,
            string subjectId,
            int accessAttributeTableId,
            AccessLevel accessLevel,
            string? grantedDataValue,
            string functionCode
        )
            : this(grantType, subjectId, accessAttributeTableId, accessLevel, grantedDataValue)
        {
            FunctionCode = functionCode;
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

        /// <summary>
        /// The function that you want to grant permission. Refer to FunctionCode class. Default value is "All".
        /// </summary>
        public string? FunctionCode { get; set; }
    }
}
