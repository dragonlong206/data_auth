using DataAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.UserRoles
{
    public class UserRoleService : IUserRoleService
    {
        DataAuthDbContext _dbContext;

        public UserRoleService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRoleModel> AddUserRole(UserRoleModel model, CancellationToken cancellationToken = default)
        {
            var entity = new UserRole(model.UserId, model.RoleId);
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync();
            model.Id = entity.Id;
            return model;
        }

    }
}
