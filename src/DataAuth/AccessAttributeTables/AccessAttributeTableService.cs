using DataAuth.Entities;
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

        public async Task<AccessAttributeTableModel> AddAccessAttributeTable(AccessAttributeTableModel model)
        {
            var entity = new AccessAttributeTable(model.AccessAttributeId, model.TableName, model.Alias, model.IdColumn, model.NameColumn
                , model.ParentColumn, model.HierarchyLevel, model.LocalPermissionTableName, model.LocalPermissionIdColumn, model.LocalPermissionLookupColumn);
            _dbContext.AccessAttributeTables.Add(entity);
            await _dbContext.SaveChangesAsync();
            model.Id = entity.Id;
            return model;
        }
    }
}
