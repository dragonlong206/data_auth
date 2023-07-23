using DataAuth.Base;
using DataAuth.Enums;

namespace DataAuth.Sample.WebApi.Models
{
    public class DataPermissionModel : BaseModel<int>
    {
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
