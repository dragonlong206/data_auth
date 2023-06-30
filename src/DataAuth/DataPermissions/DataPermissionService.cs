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

        public async Task<DataPermissionModel> AddDataPermission(DataPermissionModel model, CancellationToken cancellationToken = default)
        {
            // TODO: Validation
            var entity = new DataPermission(model.GrantType, model.SubjectId, model.AccessAttributeTableId, model.AccessLevel, model.GrantedDataValue);
            await _dbContext.DataPermissions.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;
            return model;
        }
    }
}
