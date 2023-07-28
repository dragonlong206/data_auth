using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.UserRoles
{
    public interface IUserRoleService
    {
        Task<UserRoleModel> AddUserRole(
            UserRoleModel model,
            CancellationToken cancellationToken = default
        );

        Task AddUserRoles(
            string userId,
            IEnumerable<int> roleIds,
            CancellationToken cancellationToken = default
        );

        Task<UserRoleModel> UpdateUserRole(
            int id,
            UserRoleModel model,
            CancellationToken cancellationToken = default
        );
        Task DeleteUserRole(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserRoleModel>> GetUserRoles(
            CancellationToken cancellationToken = default
        );

        Task<UserRoleModel?> GetUserRoleById(int id, CancellationToken cancellationToken = default);
    }
}
