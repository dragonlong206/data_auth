using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.UserRoles
{
    public interface IUserRoleService
    {
        Task<UserRoleModel> AddUserRole(UserRoleModel model, CancellationToken cancellationToken = default);
    }
}
