using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Roles
{
    public interface IRoleService
    {
        Task<RoleModel> AddRole(RoleModel model);
    }
}
