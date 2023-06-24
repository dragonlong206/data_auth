using DataAuth.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth
{
    public class DataAuthDbContext : DbContext
    {
        public DataAuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessAttribute>()
                .HasIndex(a => a.Code)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccessAttribute> AccessAttributes { get; set; }

        public DbSet<AccessAttributeTable> AccessAttributeTables { get; set; }

        public DbSet<DataPermission> DataPermissions { get; set; }
    }
}
