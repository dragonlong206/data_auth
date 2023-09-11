using DataAuth.Cache;
using DataAuth.Core;
using DataAuth.Entities;
using DataAuth.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Domains.DataPermissions
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

        private async Task InvalidateCache(
            DataPermission permission,
            CancellationToken cancellationToken
        )
        {
            var entity = await _dbContext.DataPermissions
                .AsNoTracking()
                .Where(x => x.Id == permission.Id)
                .Include(x => x.AccessAttributeTable)
                .ThenInclude(t => t!.AccessAttribute)
                .FirstAsync(cancellationToken);

            var cacheKey = CoreService.GetCacheKey(
                entity.SubjectId,
                entity.AccessAttributeTable!.AccessAttribute!.Code,
                entity.GrantType,
                entity.FunctionCode
            );
            _cacheProvider.Invalidate(cacheKey);
        }

        public async Task AddDataPermission(
            DataPermissionModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);
            DataPermission entity = MapModelToEntity(model);
            await _dbContext.DataPermissions.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;

            await InvalidateCache(entity, cancellationToken);
        }

        private static DataPermission MapModelToEntity(DataPermissionModel model)
        {
            return new DataPermission(
                model.GrantType,
                model.SubjectId,
                model.AccessAttributeTableId,
                model.AccessLevel,
                model.GrantedDataValue,
                model.FunctionCode
            );
        }

        // Update DataPermission
        public async Task UpdateDataPermission(
            int id,
            DataPermissionModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);

            var entity = await _dbContext.DataPermissions
                .Where(x => x.Id == id)
                .FirstAsync(cancellationToken);

            entity.GrantType = model.GrantType;
            entity.SubjectId = model.SubjectId;
            entity.AccessAttributeTableId = model.AccessAttributeTableId;
            entity.AccessLevel = model.AccessLevel;
            entity.GrantedDataValue = model.GrantedDataValue;
            entity.FunctionCode = model.FunctionCode;

            await _dbContext.SaveChangesAsync(cancellationToken);

            await InvalidateCache(entity, cancellationToken);
        }

        // Delete DataPermission
        public async Task DeleteDataPermission(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _dbContext.DataPermissions.FindAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new ObjectNotFoundException("DataPermission", id);
            }

            _dbContext.DataPermissions.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await InvalidateCache(entity, cancellationToken);
        }

        // Get DataPermission list by GrantType, SubjectId, AccessAttributeTableId, convert to DataPermissionModel
        public async Task<IEnumerable<DataPermissionModel>> GetDataPermissions(
            GrantType? grantType = null,
            string? subjectId = null,
            int? accessAttributeTableId = null,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<DataPermission> query = _dbContext.DataPermissions.OrderByDescending(
                x => x.Id
            );

            if (grantType != null)
            {
                query = query.Where(x => x.GrantType == grantType);
            }
            if (subjectId != null)
            {
                query = query.Where(x => x.SubjectId == subjectId);
            }
            if (accessAttributeTableId != null)
            {
                query = query.Where(x => x.AccessAttributeTableId == accessAttributeTableId);
            }

            return await query
                .Select(
                    x =>
                        new DataPermissionModel(x.SubjectId, x.FunctionCode)
                        {
                            Id = x.Id,
                            GrantType = x.GrantType,
                            AccessAttributeTableId = x.AccessAttributeTableId,
                            AccessLevel = x.AccessLevel,
                            GrantedDataValue = x.GrantedDataValue
                        }
                )
                .ToListAsync(cancellationToken);
        }

        // Get DataPermission by Id, convert to DataPermissionModel
        public async Task<DataPermissionModel?> GetDataPermissionById(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _dbContext.DataPermissions
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return null;
            }

            return new DataPermissionModel(entity.SubjectId)
            {
                Id = entity.Id,
                GrantType = entity.GrantType,
                AccessAttributeTableId = entity.AccessAttributeTableId,
                AccessLevel = entity.AccessLevel,
                GrantedDataValue = entity.GrantedDataValue,
                FunctionCode = entity.FunctionCode
            };
        }

        private static void ValidateModel(DataPermissionModel model)
        {
            var validator = new DataPermissionValidator();
            validator.ValidateAndThrow(model);
        }
    }
}
