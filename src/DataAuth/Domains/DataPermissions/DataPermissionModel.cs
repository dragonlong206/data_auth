using DataAuth.Base;
using DataAuth.Enums;

namespace DataAuth.Domains.DataPermissions
{
    public class DataPermissionModel : BaseModel<int>
    {
        public DataPermissionModel(string subjectId)
        {
            SubjectId = subjectId;
            FunctionCode = Enums.FunctionCode.All;
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

        /// <summary>
        /// The function that you want to grant permission. Refer to FunctionCode class. Default value is "All".
        /// </summary>
        public string FunctionCode { get; set; }
    }
}
