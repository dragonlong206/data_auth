using DataAuth.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Domains.Roles
{
    public class RoleService : IRoleService
    {
        DataAuthDbContext _dbContext;

        public RoleService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static void ValidateModel(RoleModel model)
        {
            var validator = new RoleValidator();
            validator.ValidateAndThrow(model);
        }

        public async Task<RoleModel> AddRole(
            RoleModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);
            var entity = new Role(model.Name, model.Code);

            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            model.Id = entity.Id;
            return model;
        }

        public async Task<RoleModel> UpdateRole(
            int id,
            RoleModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);
            var entity = await _dbContext.Roles.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
            if (entity == null)
            {
                throw new ObjectNotFoundException("Role", id);
            }

            entity.Name = model.Name;
            entity.Code = model.Code;
            entity.Description = model.Description;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task DeleteRole(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.Roles.FindAsync(id);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Role", id);
            }

            _dbContext.Roles.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<RoleModel?> GetRoleById(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext.Roles
                .Select(
                    x => new RoleModel(x.Name, x.Code) { Id = x.Id, Description = x.Description }
                )
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<RoleModel>> GetRoles(CancellationToken cancellationToken = default)
        {
            var entities = await _dbContext.Roles
                .Select(
                    x => new RoleModel(x.Name, x.Code) { Id = x.Id, Description = x.Description }
                )
                .ToListAsync(cancellationToken);
            return entities;
        }
    }
}
