using DataAuth.Base;
using DataAuth.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Entities
{
    public class UserRole : BaseEntity<int>
    {
        public UserRole(string userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public string UserId { get; set; }

        public int RoleId { get; set; }

        public Role? Role { get; set; }
    }
}
