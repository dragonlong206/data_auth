using DataAuth.Base;

namespace DataAuth.Domains.UserRoles
{
    public class UserRoleModel : BaseModel<int>
    {
        public UserRoleModel(string userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public string UserId { get; set; }

        public int RoleId { get; set; }
    }
}
