using DataAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAuth
{
    public class DataAuthDbContext : DbContext
    {
        public DataAuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName("DataAuth_" + entityType.GetTableName());
            }

            modelBuilder.Entity<AccessAttribute>()
                .HasIndex(a => a.Code)
                .IsUnique();

            // Make alias to be unique to avoid alias duplication when building query
            modelBuilder.Entity<AccessAttributeTable>()
                .HasIndex(a => a.Alias)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccessAttribute> AccessAttributes { get; set; }

        public DbSet<AccessAttributeTable> AccessAttributeTables { get; set; }

        public DbSet<DataPermission> DataPermissions { get; set; }
    }
}
