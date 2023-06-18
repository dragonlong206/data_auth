using DataAuth.DataAccessService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Test.WebApi.DataAccess
{
    public class ApplicationDbContext : DataAuthDbContext
    {
        public ApplicationDbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }
    }
}
