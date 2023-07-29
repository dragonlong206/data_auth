using DataAuth.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Domains.AccessAttributes
{
    public class AccessAttributeService : IAccessAttributeService
    {
        DataAuthDbContext _dbContext;

        public AccessAttributeService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static void ValidateModel(AccessAttributeModel model)
        {
            var validator = new AccessAttributeValidator();
            validator.ValidateAndThrow(model);
        }

        public async Task<AccessAttributeModel> AddAccessAttribute(
            AccessAttributeModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);

            var entity = new AccessAttribute(model.Code, model.Name, model.Description);
            await _dbContext.AccessAttributes.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;
            return model;
        }

        public async Task<AccessAttributeModel> UpdateAccessAttribute(
            int id,
            AccessAttributeModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);

            var entity = await _dbContext.AccessAttributes.FindAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new ObjectNotFoundException("AccessAttribute", id);
            }

            entity.Code = model.Code;
            entity.Name = model.Name;
            entity.Description = model.Description;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task DeleteAccessAttribute(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _dbContext.AccessAttributes.FindAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new ObjectNotFoundException("AccessAttribute", id);
            }

            _dbContext.AccessAttributes.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<AccessAttributeModel?> GetAccessAttributeById(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _dbContext.AccessAttributes.FindAsync(id, cancellationToken);
            if (entity == null)
            {
                return null;
            }

            return new AccessAttributeModel(entity.Code, entity.Name, entity.Description)
            {
                Id = entity.Id
            };
        }

        public async Task<IEnumerable<AccessAttributeModel>> GetAccessAttributes(
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext.AccessAttributes
                .Select(x => new AccessAttributeModel(x.Code, x.Name, x.Description) { Id = x.Id })
                .ToListAsync(cancellationToken);
        }
    }
}
