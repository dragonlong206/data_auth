﻿using DataAuth.Cache;
using DataAuth.Core;
using DataAuth.Entities;
using DataAuth.Enums;
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

        private async Task InvalidateCache(
            DataPermission permission,
            CancellationToken cancellationToken
        )
        {
            var entity = await _dbContext.DataPermissions
                .AsNoTracking()
                .Where(x => x.Id == permission.Id)
                .Include(x => x.AccessAttributeTable)
                .ThenInclude(t => t.AccessAttribute)
                .FirstAsync(cancellationToken);

            var cacheKey = CoreService.GetCacheKey(
                entity.SubjectId,
                entity.AccessAttributeTable!.AccessAttribute!.Code,
                entity.GrantType
            );
            _cacheProvider.Invalidate(cacheKey);
        }

        public async Task<DataPermissionModel> AddDataPermission(
            DataPermissionModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);

            var entity = new DataPermission(
                model.GrantType,
                model.SubjectId,
                model.AccessAttributeTableId,
                model.AccessLevel,
                model.GrantedDataValue
            );
            await _dbContext.DataPermissions.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;

            await InvalidateCache(entity, cancellationToken);

            return model;
        }

        // Update DataPermission
        public async Task<DataPermissionModel> UpdateDataPermission(
            DataPermissionModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);

            var entity = await _dbContext.DataPermissions
                .Where(x => x.Id == model.Id)
                .FirstAsync(cancellationToken);

            entity.GrantType = model.GrantType;
            entity.SubjectId = model.SubjectId;
            entity.AccessAttributeTableId = model.AccessAttributeTableId;
            entity.AccessLevel = model.AccessLevel;
            entity.GrantedDataValue = model.GrantedDataValue;

            await _dbContext.SaveChangesAsync(cancellationToken);

            await InvalidateCache(entity, cancellationToken);

            return model;
        }

        // Delete DataPermission
        public async Task DeleteDataPermission(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _dbContext.DataPermissions
                .Where(x => x.Id == id)
                .FirstAsync(cancellationToken);

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

            return query.Select(
                x =>
                    new DataPermissionModel
                    {
                        Id = x.Id,
                        GrantType = x.GrantType,
                        SubjectId = x.SubjectId,
                        AccessAttributeTableId = x.AccessAttributeTableId,
                        AccessLevel = x.AccessLevel,
                        GrantedDataValue = x.GrantedDataValue
                    }
            );
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

            return new DataPermissionModel
            {
                Id = entity.Id,
                GrantType = entity.GrantType,
                SubjectId = entity.SubjectId,
                AccessAttributeTableId = entity.AccessAttributeTableId,
                AccessLevel = entity.AccessLevel,
                GrantedDataValue = entity.GrantedDataValue
            };
        }

        private static void ValidateModel(DataPermissionModel model)
        {
            // Validate model, requires GrantType, SubjectId, AccessAttributeTableId, AccessLevel
            if (model.SubjectId == null)
            {
                throw new ArgumentException("SubjectId is required");
            }
            if (model.AccessAttributeTableId == 0)
            {
                throw new ArgumentException("AccessAttributeTableId is required");
            }
            // GrantedDataValue is required if AccessLevel is Deep or Specific
            if (
                (
                    model.AccessLevel == AccessLevel.Local
                    || model.AccessLevel == AccessLevel.Specific
                ) && string.IsNullOrEmpty(model.GrantedDataValue)
            )
            {
                throw new ArgumentException(
                    "GrantedDataValue is required when AccessLevel is Local"
                );
            }
        }
    }
}
