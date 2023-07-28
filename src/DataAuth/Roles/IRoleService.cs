using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Roles
{
    public interface IRoleService
    {
        Task<RoleModel> AddRole(RoleModel model, CancellationToken cancellationToken = default);

        Task<RoleModel> UpdateRole(
            int id,
            RoleModel model,
            CancellationToken cancellationToken = default
        );

        Task DeleteRole(int id, CancellationToken cancellationToken = default);

        Task<RoleModel?> GetRoleById(int id, CancellationToken cancellationToken = default);

        Task<List<RoleModel>> GetRoles(CancellationToken cancellationToken = default);
    }
}
