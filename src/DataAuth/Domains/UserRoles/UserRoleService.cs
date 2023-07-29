using DataAuth.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Domains.UserRoles
{
    public class UserRoleService : IUserRoleService
    {
        DataAuthDbContext _dbContext;

        public UserRoleService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static void ValidateModel(UserRoleModel model)
        {
            var validator = new UserRoleValidator();
            validator.ValidateAndThrow(model);
        }

        public async Task<UserRoleModel> AddUserRole(
            UserRoleModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);
            var entity = new UserRole(model.UserId, model.RoleId);
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync();
            model.Id = entity.Id;
            return model;
        }

        public async Task AddUserRoles(
            string userId,
            IEnumerable<int> roleIds,
            CancellationToken cancellationToken = default
        )
        {
            foreach (var roleId in roleIds)
            {
                var entity = new UserRole(userId, roleId);
                await _dbContext.AddAsync(entity, cancellationToken);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserRoleModel> UpdateUserRole(
            int id,
            UserRoleModel model,
            CancellationToken cancellationToken = default
        )
        {
            ValidateModel(model);
            var entity = await _dbContext.UserRoles.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
            if (entity == null)
            {
                throw new ObjectNotFoundException("UserRole", id);
            }

            entity.UserId = model.UserId;
            entity.RoleId = model.RoleId;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task DeleteUserRole(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.UserRoles.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
            if (entity == null)
            {
                throw new ObjectNotFoundException("UserRole", id);
            }

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserRoleModel>> GetUserRoles(
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext.UserRoles
                .Select(x => new UserRoleModel(x.UserId, x.RoleId) { Id = x.Id })
                .ToListAsync(cancellationToken);
        }

        public async Task<UserRoleModel?> GetUserRoleById(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext.UserRoles
                .Select(x => new UserRoleModel(x.UserId, x.RoleId) { Id = x.Id })
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
