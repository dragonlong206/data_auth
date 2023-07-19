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

        public async Task<AccessAttributeModel> AddAccessAttribute(
            AccessAttributeModel model,
            CancellationToken cancellationToken = default
        )
        {
            var entity = new AccessAttribute(model.Code, model.Name, model.Description);
            await _dbContext.AccessAttributes.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;
            return model;
        }
    }
}
