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

        public async Task<RoleModel> AddRole(RoleModel model)
        {
            throw new NotImplementedException();
        }
    }
}
