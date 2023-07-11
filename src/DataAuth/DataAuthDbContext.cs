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
                entityType.SetSchema("DataAuth");
            }

            modelBuilder.Entity<AccessAttribute>()
                .HasIndex(a => a.Code)
                .IsUnique();

            // Make alias to be unique to avoid alias duplication when building query
            modelBuilder.Entity<AccessAttributeTable>()
                .HasIndex(a => a.Alias)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(a => a.Code)
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasIndex(x => new {x.UserId, x.RoleId})
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccessAttribute> AccessAttributes { get; set; }

        public DbSet<AccessAttributeTable> AccessAttributeTables { get; set; }

        public DbSet<DataPermission> DataPermissions { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
    }
}
