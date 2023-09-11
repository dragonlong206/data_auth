using DataAuth.Entities;
using DataAuth.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAuth
{
    public class DataAuthDbContext : DbContext
    {
        public DataAuthDbContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetSchema("DataAuth");
            }

            modelBuilder.Entity<AccessAttribute>().HasIndex(a => a.Code).IsUnique();

            // Make alias to be unique to avoid alias duplication when building query
            modelBuilder.Entity<AccessAttributeTable>().HasIndex(a => a.Alias).IsUnique();

            modelBuilder.Entity<Role>().HasIndex(a => a.Code).IsUnique();

            modelBuilder.Entity<UserRole>().HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();

            modelBuilder
                .Entity<DataPermission>()
                .Property(p => p.FunctionCode)
                .IsRequired()
                .HasDefaultValue(FunctionCode.All);

            base.OnModelCreating(modelBuilder);
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.LogTo(Console.WriteLine);
        //     base.OnConfiguring(optionsBuilder);
        // }

        public virtual DbSet<AccessAttribute> AccessAttributes { get; set; }

        public virtual DbSet<AccessAttributeTable> AccessAttributeTables { get; set; }

        public virtual DbSet<DataPermission> DataPermissions { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}
