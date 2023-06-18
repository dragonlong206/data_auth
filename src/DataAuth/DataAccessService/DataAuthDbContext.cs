using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.DataAccessService
{
    public class DataAuthDbContext : DbContext
    {
        public DataAuthDbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        public DbSet<AccessAttribute> AccessAttributes { get; set; }

        public DbSet<AccessAttributeTable> AccessAttributeTables { get; set; }

        public DbSet<DataPermission> DataPermissions { get; set; }
    }
}
