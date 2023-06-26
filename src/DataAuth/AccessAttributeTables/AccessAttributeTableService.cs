using DataAuth.Entities;
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

        public async Task<AccessAttributeTableModel> AddAccessAttributeTable(AccessAttributeTableModel model, CancellationToken cancellationToken = default)
        {
            var entity = new AccessAttributeTable(model.AccessAttributeId, model.TableName, model.Alias, model.IdColumn, model.NameColumn, model.IsSelfReference
                , model.ParentColumn, model.HierarchyLevel, model.IsLeafLevel, model.LocalPermissionTableName, model.LocalPermissionIdColumn, model.LocalPermissionLookupColumn);
            _dbContext.AccessAttributeTables.Add(entity);
            await _dbContext.SaveChangesAsync();
            model.Id = entity.Id;
            return model;
        }

        public async Task<AccessAttributeTable?> GetAccessAttributeTable(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AccessAttributeTables.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
