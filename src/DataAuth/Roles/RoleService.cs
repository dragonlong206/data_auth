using DataAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Roles
{
    public class RoleService : IRoleService
    {
        DataAuthDbContext _dbContext;

        public RoleService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RoleModel> AddRole(
            RoleModel model,
            CancellationToken cancellationToken = default
        )
        {
            var entity = new Role(model.Name, model.Code);

            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;
            return model;
        }
    }
}
