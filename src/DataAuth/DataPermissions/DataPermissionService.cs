using DataAuth.Cache;
using DataAuth.Core;
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
        ICacheProvider _cacheProvider;

        public DataPermissionService(DataAuthDbContext dbContext, ICacheProvider cacheProvider)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
        }

        private async Task InvalidateCache(DataPermission permission, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.DataPermissions.AsNoTracking()
                .Where(x => x.Id == permission.Id)
                .Include(x => x.AccessAttributeTable)
                .ThenInclude(t => t.AccessAttribute)
                .FirstAsync(cancellationToken);

            var cacheKey = CoreService.GetCacheKey(entity.SubjectId, entity.AccessAttributeTable!.AccessAttribute!.Code, entity.GrantType);
            _cacheProvider.Invalidate(cacheKey);
        }

        public async Task<DataPermissionModel> AddDataPermission(DataPermissionModel model, CancellationToken cancellationToken = default)
        {
            // TODO: Validation
            var entity = new DataPermission(model.GrantType, model.SubjectId, model.AccessAttributeTableId, model.AccessLevel, model.GrantedDataValue);
            await _dbContext.DataPermissions.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;

            await InvalidateCache(entity, cancellationToken);

            return model;
        }
    }
}
