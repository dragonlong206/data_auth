using DataAuth.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.DataPermissions
{
    public class DataPermissionService : IDataPermissionService
    {
        DataAuthDbContext _dbContext;

        public DataPermissionService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataPermissionModel> AddDataPermission(DataPermissionModel model)
        {
            // TODO: Validation
            var entity = new DataPermission(model.GrantType, model.SubjectId, model.AccessAttributeTableId, model.AccessLevel, model.GrantedDataValue);
            _dbContext.DataPermissions.Add(entity);
            await _dbContext.SaveChangesAsync();
            model.Id = entity.Id;
            return model;
        }
    }
}
