using DataAuth.DataAccessService.Interface;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.DataAccessService.Implementation
{
    public class DataAuthService : IDataAuthService
    {
        DataAuthDbContext _dbContext;

        public DataAuthService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DataPermission>> GetDataPermissions(string subjectId, GrantType grantType)
        {
            return await _dbContext.DataPermissions.Where(x => x.SubjectId == subjectId && x.GrantType == grantType).ToListAsync();
        }
    }
}
