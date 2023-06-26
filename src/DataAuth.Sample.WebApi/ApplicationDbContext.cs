using DataAuth.Sample.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Sample.WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seeding data
            modelBuilder.Entity<Region>()
                .HasData(new Region("MB", "Miền Bắc"));
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Department> Departments{ get; set; }
    }
}
