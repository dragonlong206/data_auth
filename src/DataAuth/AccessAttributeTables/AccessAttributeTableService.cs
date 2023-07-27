using DataAuth.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.AccessAttributeTables
{
    public class AccessAttributeTableService : IAccessAttributeTableService
    {
        DataAuthDbContext _dbContext;

        public AccessAttributeTableService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static void Validate(AccessAttributeTableModel model)
        {
            var validator = new AccessAttributeTableValidator();
            validator.ValidateAndThrow(model);
        }

        public async Task AddAccessAttributeTable(
            AccessAttributeTableModel model,
            CancellationToken cancellationToken = default
        )
        {
            Validate(model);

            var entity = new AccessAttributeTable(
                model.AccessAttributeId,
                model.TableName,
                model.Alias,
                model.IdColumn,
                model.NameColumn,
                model.IsSelfReference,
                model.ParentColumn,
                model.HierarchyLevel,
                model.IsLeafLevel,
                model.LocalPermissionTableName,
                model.LocalPermissionIdColumn,
                model.LocalPermissionLookupColumn
            );
            await _dbContext.AccessAttributeTables.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;
        }

        public async Task UpdateAccessAttributeTable(
            int id,
            AccessAttributeTableModel model,
            CancellationToken cancellationToken = default
        )
        {
            Validate(model);

            var entity = await _dbContext.AccessAttributeTables.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
            if (entity == null)
            {
                throw new ObjectNotFoundException("AccessAttributeTable", model.Id);
            }

            entity.AccessAttributeId = model.AccessAttributeId;
            entity.TableName = model.TableName;
            entity.Alias = model.Alias;
            entity.IdColumn = model.IdColumn;
            entity.NameColumn = model.NameColumn;
            entity.IsSelfReference = model.IsSelfReference;
            entity.ParentColumn = model.ParentColumn;
            entity.HierarchyLevel = model.HierarchyLevel;
            entity.IsLeafLevel = model.IsLeafLevel;
            entity.LocalPermissionTableName = model.LocalPermissionTableName;
            entity.LocalPermissionIdColumn = model.LocalPermissionIdColumn;
            entity.LocalPermissionLookupColumn = model.LocalPermissionLookupColumn;

            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;
        }

        public async Task DeleteAccessAttributeTable(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _dbContext.AccessAttributeTables.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
            if (entity == null)
            {
                throw new ObjectNotFoundException("AccessAttributeTable", id);
            }

            _dbContext.AccessAttributeTables.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<AccessAttributeTableModel?> GetAccessAttributeTableById(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext.AccessAttributeTables
                .Select(
                    x =>
                        new AccessAttributeTableModel(
                            x.AccessAttributeId,
                            x.TableName,
                            x.Alias,
                            x.IdColumn,
                            x.NameColumn,
                            x.IsSelfReference,
                            x.ParentColumn,
                            x.HierarchyLevel,
                            x.IsLeafLevel,
                            x.LocalPermissionTableName,
                            x.LocalPermissionIdColumn,
                            x.LocalPermissionLookupColumn
                        )
                        {
                            Id = x.Id,
                        }
                )
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<AccessAttributeTable?> GetAccessAttributeTableEntityById(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext.AccessAttributeTables.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
        }

        public async Task<IEnumerable<AccessAttributeTableModel>> GetAccessAttributeTables(
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext.AccessAttributeTables
                .Select(
                    x =>
                        new AccessAttributeTableModel(
                            x.AccessAttributeId,
                            x.TableName,
                            x.Alias,
                            x.IdColumn,
                            x.NameColumn,
                            x.IsSelfReference,
                            x.ParentColumn,
                            x.HierarchyLevel,
                            x.IsLeafLevel,
                            x.LocalPermissionTableName,
                            x.LocalPermissionIdColumn,
                            x.LocalPermissionLookupColumn
                        )
                        {
                            Id = x.Id,
                        }
                )
                .ToListAsync(cancellationToken);
        }
    }
}
