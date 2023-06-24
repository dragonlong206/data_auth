using DataAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.AccessAttributes
{
    public class AccessAttributeService : IAccessAttributeService
    {
        DataAuthDbContext _dbContext;
        public AccessAttributeService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AccessAttributeModel> AddAccessAttribute(AccessAttributeModel model)
        {
            var entity = new AccessAttribute(model.Code, model.Name, model.Description);
            _dbContext.AccessAttributes.Add(entity);
            await _dbContext.SaveChangesAsync();
            model.Id = entity.Id;
            return model;
        }
    }
}
